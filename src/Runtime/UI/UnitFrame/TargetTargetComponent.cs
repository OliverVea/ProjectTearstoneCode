using MyRpg.Core.Components;
using MyRpg.Core.Events;
using UnityEngine;
using UnityEngine.UI;

namespace MyRpg.UI.UnitFrame
{
    public class TargetTargetComponent : TargetElement
    {
        [SerializeField] private Image portraitFrame;
        [SerializeField] private Image portraitImage;
        [SerializeField] private Sprite fallbackImage;

        private ITargetingEventHandler _targetingEventHandler;
        private ITargetingComponent _targetingComponent;

        protected override void OnTargetChanged(GameObject oldTarget, GameObject newTarget)
        {
            if (_targetingEventHandler != null) _targetingEventHandler.UnregisterOnNewTarget(OnTargetTargetChanged);

            _targetingEventHandler = newTarget?.GetComponent<ITargetingEventHandler>();
            if (_targetingEventHandler != null) _targetingEventHandler.RegisterOnNewTarget(OnTargetTargetChanged);

            _targetingComponent = newTarget?.GetComponent<ITargetingComponent>();
            OnTargetTargetChanged(null, _targetingComponent?.Target);
        }

        private void OnTargetTargetChanged(GameObject oldTarget, GameObject newTarget)
        {
            var hasTarget = newTarget != null;

            ShowFrame(hasTarget);
            if (hasTarget) ShowCharacterPortrait(newTarget);
        }

        private void ShowFrame(bool show)
        {
            portraitFrame.enabled = show;
            portraitImage.enabled = show;
        }

        private void ShowCharacterPortrait(GameObject newTarget)
        {
            portraitImage.sprite = fallbackImage;
            if (newTarget == null) return;
            
            var statusComponent = newTarget.GetComponent<IStatusComponent>();
            if (statusComponent == null) return;
            
            var sprite = statusComponent.GetPortrait();
            if (sprite == null) return;
            
            portraitImage.sprite = sprite;
        }
    }
}