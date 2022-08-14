using System.Linq;
using MyRpg.Core.Components;
using MyRpg.Core.Constants;
using MyRpg.Core.Models;
using UnityEngine;

namespace MyRpg.SpellBehaviour
{
    public class LesserHealBehaviour : BaseSpellBehaviour
    {
        public override GameObject GetTargetForSpell(GameObject caster, string spellId, GameObject target)
        {
            var manaComponent = caster.GetComponent<IManaComponent>();
            if (manaComponent.GetCurrentMana() <= ConstantValues.LowThreshold * manaComponent.GetMaxMana()) return null;

            var spell = LookupComponent.GetSpell(spellId);
            var effect = LookupComponent.GetEffect(spell.SpellEffectIds.Single());
                
            var castingComponent = caster.GetComponent<ICastingComponent>();
            var eligibleTargets = GetAlliesInRange(castingComponent, spellId)
                .Where(x => x.GetComponent<IHealthComponent>().GetMissingHealth() >= effect.HealingOnApplication);

            var spellTarget = eligibleTargets.FirstOrDefault();

            return spellTarget;
        }
    }
}