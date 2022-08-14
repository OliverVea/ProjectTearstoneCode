using MyRpg.Core.Components;
using MyRpg.Core.Constants;
using UnityEngine;

namespace MyRpg.SpellBehaviour
{
    internal class FireballBehaviour : BaseSpellBehaviour
    {
        public override GameObject GetTargetForSpell(GameObject caster, string spellId, GameObject target)
        {
            var castingComponent = caster.GetComponent<ICastingComponent>();
            if (!castingComponent.CanCastSpell(spellId, target)) return null;

            var cooldownComponent = caster.GetComponent<ICooldownComponent>();
            if (cooldownComponent.IsOnCooldown(spellId)) return null;
            
            var manaComponent = caster.GetComponent<IManaComponent>();
            if (manaComponent.GetCurrentMana() < ConstantValues.HighThreshold * manaComponent.GetMaxMana()) return null;

            return target;
        }
    }
}
