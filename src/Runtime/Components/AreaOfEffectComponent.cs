using System.Collections.Generic;
using System.Linq;
using MyRpg.Core.Components;
using MyRpg.Core.Constants;
using MyRpg.Core.Models;
using UnityEngine;

namespace MyRpg.Components
{
    public class AreaOfEffectComponent : MonoBehaviour, IAreaOfEffectComponent
    {
        public void ServerApplyEffect(SpellCasting spellCasting)
        {
            var targets = GetTargets(spellCasting);
            
            foreach (var target in targets)
                foreach (var effectId in spellCasting.Spell.SpellEffectIds)
                    target.GetComponent<IEffectsComponent>().ServerApplyEffect(gameObject, effectId);
        }

        private IEnumerable<GameObject> GetTargets(SpellCasting spellCasting)
        {
            var allCharacters = GameObject.FindGameObjectsWithTag("Character");

            return allCharacters.Where(x => CanBeTargeted(x, spellCasting) && IsInArea(x, spellCasting.Spell.SpellPlacement));
        }

        private bool CanBeTargeted(GameObject character, SpellCasting spellCasting)
        {
            var characterStatus = character.GetComponent<IStatusComponent>();
            var casterStatus = spellCasting.Caster.GetComponent<IStatusComponent>();

            if (characterStatus.IsDead()) return spellCasting.Spell.TargetsDead;
            
            var isFriendly = FactionRelations.IsFriendly(casterStatus.GetFaction(), characterStatus.GetFaction());
            var isAttackable = FactionRelations.IsAttackable(casterStatus.GetFaction(), characterStatus.GetFaction());

            if (isFriendly && spellCasting.Spell.CanTargetFriendly) return true;
            if (isAttackable && spellCasting.Spell.CanTargetHostile) return true;

            return false;
        }

        private bool IsInArea(GameObject character, SpellPlacement spellPlacement)
        {
            if (spellPlacement.PlacementType == SpellPlacementType.Cone)
                return IsInCone(character, spellPlacement);

            if (spellPlacement.PlacementType == SpellPlacementType.Circle)
                return IsInCircle(character, spellPlacement);

            return false;
        }

        private bool IsInCone(GameObject character, SpellPlacement spellPlacement)
            => IsInCone(character, spellPlacement.ConeDirection, spellPlacement.ConeAngle, spellPlacement.ConeRange);

        private bool IsInCone(GameObject character, Vector2 direction, float angle, float range)
        {
            var center = transform.position;
            var characterDirection = character.transform.position - center;
            var characterAngle = Mathf.Abs(Vector2.Angle(direction, characterDirection));
            return IsInCircle(character, center, range) && characterAngle < angle / 2;
        }

        private bool IsInCircle(GameObject character, SpellPlacement spellPlacement)
            => IsInCircle(character, spellPlacement.CircleCenter, spellPlacement.CircleRadius);
        
        private bool IsInCircle(GameObject character, Vector2 center, float radius)
        {
            return ((Vector2)character.transform.position - center).magnitude <= radius;
        }
    }
}