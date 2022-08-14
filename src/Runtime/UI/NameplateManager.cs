using JetBrains.Annotations;
using Mirror;
using MyRpg.Core.Components;
using MyRpg.Core.Events;
using TMPro;
using UnityEngine;

namespace MyRpg.UI
{
    public class NameplateManager : UiElement
    {
        private ITargetingComponent _targetingComponent;
        private ITargetingEventHandler _targetingEventHandler;

        protected override void OnLocalPlayerAdded(GameObject player)
        {
            _targetingComponent = NetworkClient.localPlayer.GetComponent<ITargetingComponent>();
            _targetingEventHandler = NetworkClient.localPlayer.GetComponent<ITargetingEventHandler>();
            
            _targetingEventHandler.RegisterOnNewTarget(OnTargetChanged);
            _targetingEventHandler.RegisterOnNewHoverTarget(OnHoverTargetChanged);
        }

        protected override void OnLocalPlayerRemoved(GameObject player)
        {
            _targetingEventHandler.UnregisterOnNewTarget(OnTargetChanged);
            _targetingEventHandler.UnregisterOnNewHoverTarget(OnHoverTargetChanged);
        }

        private void OnTargetChanged(GameObject oldTarget, GameObject newTarget)
        {
            var hoverTarget = _targetingComponent.HoverTarget;

            if (oldTarget != hoverTarget) ShowNameplate(oldTarget, false);
            ShowNameplate(newTarget, true);
        }

        private void OnHoverTargetChanged(GameObject oldHoverTarget, GameObject newHoverTarget)
        {
            var target = _targetingComponent.Target;

            if (oldHoverTarget != target) ShowNameplate(oldHoverTarget, false);
            ShowNameplate(newHoverTarget, true);
        }

        private void ShowNameplate([CanBeNull] GameObject target, bool state)
        {
            if (target == null) return;

            var characterName = target.GetComponent<IStatusComponent>()?.GetName();
            
            var targetTexts = target.GetComponentsInChildren<TMP_Text>();

            foreach (var text in targetTexts)
            {
                if (!text.CompareTag("Nameplate")) continue;

                text.text = characterName;
                text.enabled = state;
                return;
            }
        }
        
        
    }
}