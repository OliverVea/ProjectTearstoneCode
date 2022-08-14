using System.Linq;
using MyRpg.Core.Components;
using MyRpg.Core.Constants;
using MyRpg.Core.Models;
using UnityEngine;

namespace MyRpg.SpellBehaviour
{
    internal class RenewBehaviour : BaseSpellBehaviour
    {
        public override GameObject GetTargetForSpell(GameObject caster, string spellId, GameObject target)
        {
            var manaComponent = caster.GetComponent<IManaComponent>();
            if (manaComponent.GetCurrentMana() <= ConstantValues.LowThreshold * manaComponent.GetMaxMana()) return null;
            
            var effect = LookupComponent.GetEffect(spellId);
            var totalHealing = effect.HealingPerSecond * effect.Duration;
                
            var castingComponent = caster.GetComponent<ICastingComponent>();
            var eligibleTargets = GetAlliesInRange(castingComponent, spellId)
                .Where(x => x.GetComponent<IHealthComponent>().GetMissingHealth() >= totalHealing * 0.75f)
                .Where(x => x.GetComponent<IEffectsComponent>().GetEffectApplication(effect.Id) == null);

            var spellTarget = eligibleTargets.FirstOrDefault();

            return spellTarget;
        }
    }
}
