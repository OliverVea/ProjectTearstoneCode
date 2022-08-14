using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Mirror;
using MyRpg.Core;
using MyRpg.Core.Animation;
using MyRpg.Core.Attributes;
using MyRpg.Core.Components;
using MyRpg.Core.Constants;
using MyRpg.Core.Models;
using MyRpg.Core.Modifiers;
using UnityEngine;

namespace MyRpg.Components
{
    public class CastingComponent : NetworkBehaviour, ICastingComponent
    {
        [field: SerializeField] [field: ReadOnly] public List<Spell> AvailableSpells { get; private set; }

        private ITargetingComponent _targetingComponent;
        private IStatusComponent _statusComponent;
        private IProgressComponent _progressComponent;
        private IManaComponent _manaComponent;
        [CanBeNull] private ICharacterAnimationController _characterAnimationController;
        private IAreaOfEffectComponent _areaOfEffectComponent;
        private ICooldownComponent _cooldownComponent;
        private IChargeComponent _chargeComponent;
        private ICharacterStatusProvider[] _characterStatusProviders;
        private IAttackingComponent _attackingComponent;

        private IPlayerController _playerController;

        private void Awake()
        {
            _targetingComponent = GetRequiredComponent<ITargetingComponent>();
            _statusComponent = GetRequiredComponent<IStatusComponent>();
            _progressComponent = GetRequiredComponent<IProgressComponent>();
            _manaComponent = GetRequiredComponent<IManaComponent>();
            _areaOfEffectComponent = GetRequiredComponent<IAreaOfEffectComponent>();
            _cooldownComponent = GetRequiredComponent<ICooldownComponent>();
            _chargeComponent = GetRequiredComponent<IChargeComponent>();
            _characterAnimationController = GetComponent<ICharacterAnimationController>();
            _attackingComponent = GetRequiredComponent<IAttackingComponent>();
            _characterStatusProviders = GetComponents<ICharacterStatusProvider>();

            _playerController = GetComponent<IPlayerController>();
        }

        [Command]
        public void CommandCast(string spellId, GameObject target)
            => ServerCast(spellId, target);

        [Server]
        public void ServerCast(string spellId, GameObject target)
        {
            var spell = AvailableSpells.FirstOrDefault(x => x.Id == spellId);
            if (spell == null) return;

            target = GetTarget(spell, target);

            if (FactionRelations.IsHostile(target.GetComponent<IStatusComponent>().GetFaction(),
                    _statusComponent.GetFaction()))
                _attackingComponent.IsAttacking = true;

            if (!CanCastSpell(spellId, target)) return;

            if (spell.IsAreaOfEffect)
            {
                if (spell.SpellPlacement.CircleCenteredOnPlayer) spell.SpellPlacement.CircleCenter = transform.position;
                
                else throw new NotImplementedException("spell.IsAreaOfEffect && !spell.SpellPlacement.CircleCenteredOnPlayer");
            }

            var spellCasting = new SpellCasting
            {
                SpellId = spellId,
                Caster = gameObject,
                Target = target
            };
            
            Action spellExecutionAction = () => ServerExecuteSpell(spellCasting);
            // Action spellCancelledAction = () => { };
            
            // Action spellInitiatedAction = () => { };
            // Action spellEndedAction = () => { FindObjectsOfType<>()}; // <- We'll use this for channeled spells.

            if (spell.CastTime <= 0 || spell.IsChanneled) spellExecutionAction.Invoke();
            else _progressComponent.ServerStartProgress(
                spell.CastTime, 
                spellExecutionAction, 
                spell.Name, 
                spell.Interruptible);
        }

        public bool CanCastSpell(string spellId, GameObject target)
        {
            var spell = LookupComponent.GetSpell(spellId);

            if (spell == null) return false;
            if (_manaComponent.GetCurrentMana() < spell.ManaCost) return false;
            if (_cooldownComponent.IsOnCooldown(spell.Id)) return false;
            if (_characterStatusProviders.Any(x => !x.GetCharacterStatus().CanCast)) return false;
            if (target == null) return false;
            if ((target.transform.position - transform.position).magnitude > spell.Range) return false;
            if (!HasLineOfSight(target)) return false;
            if (!spell.TargetsDead && (target.GetComponent<IStatusComponent>()?.IsDead() ?? false)) return false;
            if (SpellIsBlocked(spell, target)) return false;

            return true;
        }

        private bool HasLineOfSight(GameObject target)
        {
            var hits = Physics2D.LinecastAll(transform.position, target.transform.position);
            return !hits.Any(hit => hit.transform.gameObject.CompareTag("Wall"));
        }

        private bool SpellIsBlocked(Spell spell, GameObject target)
        {
            var spellBlockedProviders = target.GetComponents<ISpellBlockedProvider>();
            var areBlocked = spellBlockedProviders.Select(x => x.IsSpellBlocked(spell.Id));
            var isBlocked = areBlocked.Any(x => x);
            return isBlocked;
        }

        [Server]
        public void ServerExecuteSpell(SpellCasting spellCasting)
        {
            if (!CanCastSpell(spellCasting.Spell.Id, spellCasting.Target)) return;
            
            Debug.Log($"{spellCasting.Caster} casting {spellCasting.Spell.Name} on {spellCasting.Target}.");
            
            _characterAnimationController.ServerTriggerSpellCast();
            
            if (spellCasting.IsProjectile && spellCasting.IsAreaOfEffect)
            {
                // Spawns AOE gameobject, which, usually, does damage over time.
                
                // Frozen Orb - Projectile, Instant, AOE
                // Death and Decay - Projectile, Instant, AOE
                // Flamestrike - Projectile, Casted, AOE
                
                // Blizzard - Projectile, Channeled, AOE
                throw new NotImplementedException("spellCasting.IsProjectile && spellCasting.IsAreaOfEffect");
            }

            if (!spellCasting.IsProjectile && spellCasting.IsAreaOfEffect)
            {
                // AOE but calculates targets immediately and applies effects.
                
                // Efflorescence - Immediate, Instant, AOE
                // Prayer of Healing - Immediate, Casted, AOE
                // Frost Nova - Immediate, Instant, AOE
                // Cone of Cold - Immediate, Instant, AOE
                
                // Boss Aoe #45 - Immediate, Channeled, AOE

                if (spellCasting.IsChanneled)
                    throw new NotImplementedException(
                        "!spellCasting.IsProjectile && spellCasting.IsAreaOfEffect && spellCasting.IsChanneled");
                
                _areaOfEffectComponent.ServerApplyEffect(spellCasting);
            }

            if (spellCasting.IsProjectile && !spellCasting.IsAreaOfEffect)
            {
                // Spawns single-target projectile with a specific target. I don't think this can be channeled, unless you count e.g. charge.
                
                // Ice lance - Projectile, Instant, Single-target
                // Frostbolt - Projectile, Casted, Single-target
                
                var projectileComponent = ServerSpawnProjectile(spellCasting);
            }

            if (!spellCasting.IsProjectile && !spellCasting.IsAreaOfEffect)
            {
                // Single target and immediate. 
                
                // Power Word: Shield - Immediate, Instant, Single-target
                // Smite - Immediate, Casted, Single-target
                // Shadow Word: Pain - Immediate, Instant, Single-target
                
                // Drain Life - Immediate, Channeled, Single-target

                if (spellCasting.Spell.IsCharge) _chargeComponent.ServerSetCharge(spellCasting.Target, spellCasting.Spell.SpellEffectIds.FirstOrDefault());
                else
                {
                    foreach (var effectId in spellCasting.Spell.SpellEffectIds)
                        spellCasting.Target.GetComponent<IEffectsComponent>().ServerApplyEffect(gameObject, effectId);
                }
            }

            _manaComponent.ServerSpendMana(spellCasting.Spell.ManaCost);
            _cooldownComponent.ServerAddToCooldown(spellCasting.Spell.Id);
        }

        private GameObject GetTarget(Spell spell, GameObject target)
        {
            target ??= _targetingComponent.Target;
            var targetStatus = target == null ? null : target.GetComponent<IStatusComponent>();

            var hasValidTarget = targetStatus != null;

            if (hasValidTarget)
            {
                var canTargetTarget = !targetStatus.IsDead() || spell.TargetsDead;

                if (canTargetTarget)
                {
                    var targetingHostile = TargetingHostile(spell, targetStatus);
                    var targetingFriendly = TargetingFriendly(spell, targetStatus);

                    if (targetingHostile || targetingFriendly)
                        return target;
                }
            }

            if (TargetingSelf(spell)) 
                return gameObject;

            return TargetingNearby(spell);
        }

        private GameObject TargetingNearby(Spell spell)
        {
            if (_playerController == null ||
                spell.TargetsDead || 
                spell.CanTargetFriendly || 
                spell.CanTargetSelf || 
                !spell.CanTargetHostile) return null;

            var target = _playerController.GetNearbyEnemies().FirstOrDefault();

            if (target == null) return null;

            _targetingComponent.ServerSetTarget(target);
            return target;
        }

        private bool TargetingHostile(Spell spell, IStatusComponent targetStatus)
        {
            return spell.CanTargetHostile && 
                   targetStatus != null && 
                   FactionRelations.IsAttackable(_statusComponent.GetFaction(), targetStatus.GetFaction());
        }

        private bool TargetingFriendly(Spell spell, IStatusComponent targetStatus)
        {
            return spell.CanTargetFriendly && 
                   targetStatus != null && 
                   FactionRelations.IsFriendly(_statusComponent.GetFaction(), targetStatus.GetFaction());
        }

        private bool TargetingSelf(Spell spell)
            => spell.CanTargetSelf || spell.SpellPlacement != null && spell.SpellPlacement.CircleCenteredOnPlayer;

        public void ServerInitialize(Character character)
        {
            AvailableSpells = GetAvailableSpells(character.characterClass);
        }

        private static List<Spell> GetAvailableSpells(Class characterClass)
        {
            string[] spellIds = characterClass switch
            {
                Class.Mage => new[] { "fireball", "frostbolt", "frostnova" },
                Class.Warrior => new[] { "charge", "shieldbash", "taunt" },
                Class.Priest => new[] { "lesserheal", "renew", "powerwordshield" },
                _ => Array.Empty<string>()
            };

            return spellIds.Select(LookupComponent.GetSpell).ToList();
        }
        
        
        [Server]
        private IProjectileComponent ServerSpawnProjectile(SpellCasting spellCasting)
        {
            var projectileComponent = SpawnProjectileObject(spellCasting);
            BroadcastSpawnProjectile(spellCasting);
            return projectileComponent;
        }

        [ClientRpc]
        private void BroadcastSpawnProjectile(SpellCasting spellCasting)
        {
            if (isServer) return;
            SpawnProjectileObject(spellCasting);
        }

        private IProjectileComponent SpawnProjectileObject(SpellCasting spellCasting)
        {
            var sourcePosition = transform.position;
            var direction = Quaternion.FromToRotation(Vector2.right, spellCasting.Target.transform.position - sourcePosition);
            var projectileObject = Instantiate(spellCasting.Spell.Projectile, sourcePosition + Vector3.up * 1f, direction);
            
            var projectileComponent = projectileObject.GetComponent<IProjectileComponent>();
            projectileComponent.Initialize(spellCasting.Spell.SpellEffectIds.FirstOrDefault(), gameObject, spellCasting.Target);

            return projectileComponent;
        }
    }
}