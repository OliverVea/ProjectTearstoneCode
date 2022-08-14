using MyRpg.Core.Events;
using UnityEngine;

namespace MyRpg.UI.UnitFrame
{
    public class TargetPortraitComponent : AbstractPortraitComponent
    {
        private ITargetingEventHandler _targetingEventHandler;

        protected override void OnLocalPlayerAdded(GameObject player)
        {
            _targetingEventHandler = player.GetComponent<ITargetingEventHandler>();
            
            _targetingEventHandler.RegisterOnNewTarget(OnTargetChanged);
        }

        protected override void OnLocalPlayerRemoved(GameObject player)
        {
            _targetingEventHandler.UnregisterOnNewTarget(OnTargetChanged);
        }

        private void OnTargetChanged(GameObject oldTarget, GameObject newTarget)
            => SetTarget(newTarget);
    }
}