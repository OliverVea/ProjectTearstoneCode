using MyRpg.Core.Components;
using MyRpg.Core.Events;
using UnityEngine;

namespace MyRpg.UI.UnitFrame.Effects
{
    public class PlayerEffectTrackers : EffectTrackers
    {
        private IEffectsEventHandler _effectsEventHandler;
        
        private IEffectsComponent _effectsComponent;

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
            effectTracker.Initialize(_effectsComponent, effectId);
        }

        protected override void OnLocalPlayerAdded(GameObject player)
        {
            _effectsEventHandler = player.GetComponent<IEffectsEventHandler>();
            _effectsComponent = player.GetComponent<IEffectsComponent>();
            
            _effectsEventHandler.RegisterOnEffectApplied(OnEffectApplied);
            _effectsEventHandler.RegisterOnEffectExpired(OnEffectRemoved);
            _effectsEventHandler.RegisterOnEffectRemoved(OnEffectRemoved);
        }

        protected override void OnLocalPlayerRemoved(GameObject player)
        {
            _effectsEventHandler.UnregisterOnEffectApplied(OnEffectApplied);
            _effectsEventHandler.UnregisterOnEffectExpired(OnEffectRemoved);
            _effectsEventHandler.UnregisterOnEffectRemoved(OnEffectRemoved);
        }
    }
}