using UnityEngine;

namespace MyRpg.UI.UnitFrame
{
    public class PlayerPortraitComponent : AbstractPortraitComponent
    {
        protected override void OnLocalPlayerAdded(GameObject player)
        {
            SetTarget(player);
        }
    }
}