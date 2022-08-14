using UnityEngine;

namespace MyRpg.UI.UnitFrame.Bars.Progress
{
    public class PlayerProgressBar : AbstractProgressBar
    {
        protected override void OnLocalPlayerAdded(GameObject player)
            => SetTarget(player);

        protected override void OnLocalPlayerRemoved(GameObject player)
            => ClearTarget();
    }
}
