﻿using MyRpg.Core.Events;
using UnityEngine;

namespace MyRpg.UI
{
    public abstract class TargetElement : UiElement
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

        protected abstract void OnTargetChanged(GameObject oldTarget, GameObject newTarget);
    }
}