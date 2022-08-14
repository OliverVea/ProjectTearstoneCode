using System;
using System.Collections.Generic;
using Mirror;
using System.Linq;
using JetBrains.Annotations;
using MyRpg.Core.Components;
using MyRpg.Core.Constants;
using MyRpg.Core.Events;
using MyRpg.Core.Models;
using MyRpg.Core.Modifiers;
using UnityEngine;

namespace MyRpg.Components
{
    public partial class EffectsComponent : NetworkBehaviour, IEffectsComponent, ICharacterStatusProvider, 
        IAbsorptionProvider, ISpellBlockedProvider, ITauntedProvider
    {
        
        private readonly SyncList<EffectApplication> _effectApplications = new SyncList<EffectApplication>();
        [SerializeField] private List<EffectApplication> effectApplicationsMirror;

        private IDeathEventHandler _deathEventHandler;
        
        private IHealthComponent _healthComponent;
        private IAnimationTargetComponent _animationTargetComponent;
        private IProgressComponent _progressComponent;
        [CanBeNull] private IThreatComponent _threatComponent;


        private void Awake()
        {
            _deathEventHandler = GetRequiredComponent<IDeathEventHandler>();
            
            _healthComponent = GetRequiredComponent<IHealthComponent>();
            _animationTargetComponent = GetRequiredComponent<IAnimationTargetComponent>();
            _threatComponent = GetComponent<IThreatComponent>();
            _progressComponent = GetComponent<IProgressComponent>();
        }

        private void OnEnable()
        {
            _deathEventHandler.RegisterOnDeath(OnDeath);
        }

        private void OnDisable()
        {
            _deathEventHandler.UnregisterOnDeath(OnDeath);
        }

        private void OnDeath()
        {
            if (!isServer) return;
            _effectApplications.RemoveAll(effectApplication => effectApplication.Effect.RemoveOnDeath);
        }

        private void Update()
        {
            if (isServer)
            {
                foreach (var ea in _effectApplications) ServerUpdateEffect(ea);
                ServerExpireEffects();
            }
            
            effectApplicationsMirror = _effectApplications.ToList();
        }

        [Server]
        private void ServerUpdateEffect(EffectApplication effectApplication)
        {
            var effect = effectApplication.Effect;
            ServerApplyDamage(effectApplication.owner, effect.DamagePerSecond * Time.deltaTime, effect.Lifesteal);
            ServerApplyHealing(effectApplication.owner, effect.HealingPerSecond * Time.deltaTime);

            var timeActive = effectApplication.timeActive + Time.deltaTime;
            UpdateTimeActive(effectApplication.owner, effectApplication, timeActive);
        }

        private void UpdateTimeActive(GameObject owner, EffectApplication effectApplication, float timeActive)
        {
            var newEffectApplication = new EffectApplication 
            {
                effectId = effectApplication.effectId, 
                owner = effectApplication.owner, 
                timeActive = timeActive,
                absorbedDamage = effectApplication.absorbedDamage
            };

            if (!_effectApplications.Contains(effectApplication))
            {
                _effectApplications.Add(newEffectApplication);
                return;
            }

            var i = _effectApplications.IndexOf(effectApplication);
            _effectApplications[i] = newEffectApplication;
        }

        [Server]
        private void ServerExpireEffects()
        {
            var expiredEffectApplications = _effectApplications.Where(IsExpired);
            foreach (var effectApplication in expiredEffectApplications) ServerExpireEffect(effectApplication);
            
            _effectApplications.RemoveAll(effectApplication => IsExpired(effectApplication));
        }
        
        private static bool IsExpired(EffectApplication effectApplication)
            => effectApplication.timeActive >= effectApplication.Effect.Duration;

        private static bool IsNotExpired(EffectApplication effectApplication)
            => !IsExpired(effectApplication);

        [Server]
        private void ServerExpireEffect(EffectApplication effectApplication)
        {
            var effect = effectApplication.Effect;
            ServerApplyDamage(effectApplication.owner, effect.DamageOnExpiry, effect.Lifesteal);
            ServerApplyHealing(effectApplication.owner, effect.HealingOnExpiry);
            
            ServerOnEffectExpired(effectApplication.effectId);
            BroadcastOnEffectExpired(effectApplication.effectId);
        }

        [Server]
        private void ServerApplyDamage(GameObject owner, float damage, float lifesteal)
        {
            if (damage <= 0) return;
            var healthBefore = _healthComponent.GetCurrentHealth();
            _healthComponent?.ServerTakeDamage(owner, damage);
            var healthAfter = _healthComponent.GetCurrentHealth();
            var damageTaken = healthBefore - healthAfter;

            if (owner == null) return;
            if (damageTaken > 0) owner.GetComponent<IHealthComponent>()?.ServerTakeHealing(owner, damageTaken * lifesteal);
        }
        
        [Server]
        private void ServerApplyHealing(GameObject owner, float healing)
        {
            if (healing != 0) _healthComponent?.ServerTakeHealing(owner, healing); 
        }


        public float GetMovementSpeedModifier()
        {
            var modifier = 1.0f;

            if (IsFrozen()) return 0f;

            if (!_effectApplications.Any()) return modifier;
            
            var positiveEffects = _effectApplications.Select(ea => ea.Effect)
                .Where(e => e.SpeedBoost > 0)
                .Select(e => e.SpeedBoost);
            
            var negativeEffects = _effectApplications.Select(ea => ea.Effect)
                .Where(e => e.Slow != 0)
                .Select(e => e.Slow);

            foreach (var v in positiveEffects) modifier += v;
            foreach (var v in negativeEffects) modifier *= 1 - v;

            if (IsChilled()) modifier *= ConstantValues.ChilledMovementReduction;
            
            var lowerLimit = _effectApplications.Any() ? 
                _effectApplications.Select(ea => ea.Effect).Max(x => x.LowerSpeedLimit) : 0;

            modifier = Mathf.Clamp(modifier, lowerLimit, ConstantValues.MaxMovementModifier);
            
            return modifier;
        }

        public Sprite GetIconFromId(string effectId)
        {
            var effectApplication = GetEffectApplication(effectId);
            return effectApplication.Effect?.Icon;
        }

        public IEnumerable<EffectApplication> GetEffectApplications()
        {
            return _effectApplications.ToArray();
        }

        public bool HasAbsorption()
        {
            return _effectApplications.Any(x => x.Effect.Absorption > x.absorbedDamage);
        }

        public EffectApplication GetEffectApplication(string effectId)
        {
            return _effectApplications.FirstOrDefault(effectApplication => effectApplication.effectId == effectId);
        }

        private bool IsFrozen()
            => _effectApplications.Any(x => x.Effect.SetFrozen);

        private bool IsChilled()
            => _effectApplications.Any(x => x.Effect.SetChilled);

        public void ServerRemoveEffect(string effectId)
        {
            _effectApplications.RemoveAll(x => x.effectId == effectId);

            ServerOnEffectRemoved(effectId);
            BroadcastOnEffectRemoved(effectId);
        }

        public bool IsStunned()
            => _effectApplications.Any(x => x.Effect.SetStunned);

        [Server]
        public void ServerApplyEffect(GameObject owner, string effectId)
        {
            var effectApplication = new EffectApplication
            {
                effectId = effectId, 
                owner = owner
            };

            if (effectApplication.Effect == null) throw new NotSupportedException("Effect has not been implemented.");
            
            if (effectApplication.Effect.Interrupts)
                if (!_progressComponent.ServerInterrupt())
                    return;
            
            PlayOnHitAnimation(effectId);
            BroadcastPlayOnHitAnimation(effectId);

            if (effectApplication.Effect.Duration > 0)
            {
                var currentEffectApplication = _effectApplications.SingleOrDefault(x => x.effectId == effectId);
                if (currentEffectApplication != null)
                {
                    var currentEffect = currentEffectApplication.Effect;
                    ServerApplyDamage(currentEffectApplication.owner, currentEffect.DamageOnApplication, currentEffect.Lifesteal);
                    ServerApplyHealing(currentEffectApplication.owner, currentEffect.HealingOnApplication);
                
                    UpdateTimeActive(owner, currentEffectApplication, 0);
                    return;
                }
            
                _effectApplications.Add(effectApplication);
                
                ServerOnEffectApplied(effectId);
                BroadcastOnEffectApplied(effectId);
            }

            var effect = effectApplication.Effect;
            ServerApplyDamage(effectApplication.owner, effect.DamageOnApplication, effect.Lifesteal);
            ServerApplyHealing(effectApplication.owner, effect.HealingOnApplication);
            if (effectApplication.Effect.Taunts) ServerTaunt(effectApplication);
            if (effectApplication.Effect.FocusesCaster)
            {
                var casterThreatComponent = effectApplication.owner.GetComponent<IThreatComponent>();
                casterThreatComponent.ServerTaunt(gameObject);
            }
        }

        private void ServerTaunt(EffectApplication effectApplication)
        {
            _threatComponent?.ServerTaunt(effectApplication.owner);
        }

        [Server]
        public void ServerRemoveEffect(EffectType effectType)
        {
            var toRemove = GetFirstEffectApplicationOfType(effectType);
            if (toRemove == null) return;
            
            ServerApplyDamage(toRemove.owner, toRemove.Effect.DamageOnRemoval, toRemove.Effect.Lifesteal);
            ServerApplyHealing(toRemove.owner, toRemove.Effect.HealingOnRemoval);

            _effectApplications.RemoveAll(effectApplication => effectApplication.effectId == toRemove.effectId);

            ServerOnEffectRemoved(toRemove.effectId);
            BroadcastOnEffectRemoved(toRemove.effectId);
        }

        private EffectApplication GetFirstEffectApplicationOfType(EffectType effectType)
        {
            return _effectApplications.FirstOrDefault(effectApplication => effectApplication.Effect.EffectType == effectType);
        }

        // ICharacterStatusProvider
        public CharacterStatus GetCharacterStatus()
        {
            return new CharacterStatus
            {
                CanAttack = !_effectApplications.Any(x => x.Effect.SetStunned),
                CanMove = !_effectApplications.Any(x => x.Effect.SetFrozen || x.Effect.SetStunned),
                CanCast = !_effectApplications.Any(x => x.Effect.SetStunned || x.Effect.SetSilenced)
            };
        }
        
        // BROADCASTS

        [ClientRpc]
        private void BroadcastPlayOnHitAnimation(string effectId)
        {
            if (isServer) return;
            PlayOnHitAnimation(effectId);
        }

        private void PlayOnHitAnimation(string effectId)
        {
            var effect = LookupComponent.GetEffect(effectId);
            if (effect?.EffectOnImpact == null) return;
            
            _animationTargetComponent.PlayEffect(effect.EffectOnImpact, effectId);
        }

        [Server]
        public float ServerAbsorbDamage(float damage)
        {
            for (var i = 0; i < _effectApplications.Count; i++)
            {
                if (damage <= 0) break;
                
                var effectApplication = _effectApplications[i];
                var effectAbsorption = effectApplication.Effect.Absorption;
                if (effectAbsorption <= 0) continue;

                var totalAbsorbedDamage = Mathf.Min(effectAbsorption, effectApplication.absorbedDamage + damage);
                var absorbedDamage = totalAbsorbedDamage - effectApplication.absorbedDamage;

                if (totalAbsorbedDamage >= effectAbsorption)
                {
                    _effectApplications.Remove(effectApplication);
                    
                    ServerOnEffectRemoved(effectApplication.effectId);
                    BroadcastOnEffectRemoved(effectApplication.effectId);
                }
                else
                {
                    _effectApplications[i] = new EffectApplication
                    {
                        owner = effectApplication.owner,
                        absorbedDamage = totalAbsorbedDamage,
                        effectId = effectApplication.effectId,
                        timeActive = effectApplication.timeActive
                    };
                }

                damage -= absorbedDamage;
            }

            return damage;
        }

        public bool IsSpellBlocked(string spellId)
        {
            return _effectApplications.Any(x => x.Effect.BlocksSpells.Contains(spellId));
        }

        public GameObject GetTaunter()
        {
            var tauntEffects = _effectApplications
                .OrderBy(x => x.timeActive)
                .FirstOrDefault(x => x.Effect.Taunts);
            
            return tauntEffects?.owner;
        }

        public void ServerInitialize(Character character)
        {
            
        }
    }
}