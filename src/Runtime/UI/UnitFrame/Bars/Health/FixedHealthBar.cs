using MyRpg.Core.UI;
using UnityEngine;

namespace MyRpg.UI.UnitFrame.Bars
{
    public class FixedHealthBar : AbstractHealthBar, IFixedTargetComponent
    {
        public void Initialize(GameObject target)
            => base.SetTarget(target);
    }
}