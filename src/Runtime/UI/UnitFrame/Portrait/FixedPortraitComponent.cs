using MyRpg.Core.UI;
using UnityEngine;

namespace MyRpg.UI.UnitFrame
{
    public class FixedPortraitComponent : AbstractPortraitComponent, IFixedTargetComponent
    {
        public void Initialize(GameObject target)
            => SetTarget(target);
    }
}