using UnityEngine;

namespace MyRpg.SpellBehaviour
{
    public class ChargeBehaviour : BaseSpellBehaviour
    {
        public override GameObject GetTargetForSpell(GameObject caster, string spellId, GameObject target)
        {
            var casterPosition = caster.transform.position;
            var targetPosition = target.transform.position;
            var distance = (casterPosition - targetPosition).magnitude;
            if (distance < 4f) return null;

            return target;
        }
    }
}