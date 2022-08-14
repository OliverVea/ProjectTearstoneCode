using System.Linq;
using MyRpg.Core.Components;
using MyRpg.Core.Models;
using UnityEngine;

namespace MyRpg.SpellBehaviour
{
    public class DeathStrikeBehaviour : BaseSpellBehaviour
    {
        public override GameObject GetTargetForSpell(GameObject caster, string spellId, GameObject target)
        {
            var spell = LookupComponent.GetSpell(spellId);
            var effect = LookupComponent.GetEffect(spell.SpellEffectIds.First());

            var healthComponent = caster.GetComponent<IHealthComponent>();
            if (healthComponent.GetMissingHealth() < effect.DamageOnApplication * effect.Lifesteal) return null;

            return target;
        }
    }
}