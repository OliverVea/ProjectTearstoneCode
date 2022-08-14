using MyRpg.Core.Components;
using UnityEngine;

namespace MyRpg.SpellBehaviour
{
    public class ShieldBashBehaviour : BaseSpellBehaviour
    {
        public override GameObject GetTargetForSpell(GameObject caster, string spellId, GameObject target)
        {
            var targetProgressComponent = target.GetComponent<IProgressComponent>();
            if (targetProgressComponent == null) return null;

            var targetProgress = targetProgressComponent.GetProgress();
            if (targetProgress == null || !targetProgress.isInterruptible) return null;

            if (targetProgress.progress / targetProgress.finishTime < 0.5f) return null;

            return target;
        }
    }
}