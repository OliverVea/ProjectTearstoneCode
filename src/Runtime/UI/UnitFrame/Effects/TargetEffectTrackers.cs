using Mirror;
using MyRpg.Core.Components;
using MyRpg.Core.Events;
using UnityEngine;

namespace MyRpg.UI.UnitFrame.Effects
{
    public class TargetEffectTrackers : EffectTrackers
    {
        private ITargetingEventHandler _targetingEventHandler;
        
        private IEffectsComponent _targetEffectsComponent;

        protected override void OnLocalPlayerAdded(GameObject player)
        {
            _targetingEventHandler = NetworkClient.localPlayer.GetComponent<ITargetingEventHandler>();
            
            _targetingEventHandler.RegisterOnNewTarget(OnNewTarget);
        }

        protected override void OnLocalPlayerRemoved(GameObject player)
        {
            _targetingEventHandler.UnregisterOnNewTarget(OnNewTarget);
        }

        private void OnNewTarget(GameObject oldTarget, GameObject newTarget)
        {
            if (oldTarget != null)
            {
                var oldTargetEventHandler = oldTarget.GetComponent<IEffectsEventHandler>();
            
                oldTargetEventHandler.UnregisterOnEffectApplied(OnEffectApplied);
                oldTargetEventHandler.UnregisterOnEffectExpired(OnEffectRemoved);
                oldTargetEventHandler.UnregisterOnEffectRemoved(OnEffectRemoved);
                
                ClearEffects();
            }

            if (newTarget != null)
            {
                var newTargetEventHandler = newTarget.GetComponent<IEffectsEventHandler>();
            
                newTargetEventHandler.RegisterOnEffectApplied(OnEffectApplied);
                newTargetEventHandler.RegisterOnEffectExpired(OnEffectRemoved);
                newTargetEventHandler.RegisterOnEffectRemoved(OnEffectRemoved);

                _targetEffectsComponent = newTarget.GetComponent<IEffectsComponent>();
                
                InitializeEffects();
            }
        }

        private void InitializeEffects()
        {
            var effectApplications = _targetEffectsComponent.GetEffectApplications();
            foreach (var effectApplication in effectApplications) AddEffect(effectApplication.effectId);
        }

        private void OnEffectApplied(string effectId)
        {
            AddEffect(effectId);
        }

        private void OnEffectRemoved(string effectId)
        {
            RemoveEffect(effectId);
        }

        protected override void InitializeEffectTracker(EffectTracker effectTracker, string effectId)
        {
            effectTracker.Initialize(_targetEffectsComponent, effectId);
        }
    }
}