using UnityEngine;

namespace MyRpg.UI.UnitFrame.Bars
{
    public class PlayerManaBar : AbstractManaBar
    {
        protected override void OnLocalPlayerAdded(GameObject player)
            => SetTarget(player);

        protected override void OnLocalPlayerRemoved(GameObject player)
            => ClearTarget();
    }
}
