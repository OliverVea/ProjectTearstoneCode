using System;
using Mirror;
using MyRpg.Core.Events;

namespace MyRpg.Components
{
    public partial class EffectsComponent : IEffectsEventHandler
    {
        private event Action<string> EffectApplied;
        private event Action<string> EffectRemoved;
        private event Action<string> EffectExpired;
        
        public void RegisterOnEffectApplied(Action<string> callback)
            => EffectApplied += callback;

        public void UnregisterOnEffectApplied(Action<string> callback)
            => EffectApplied -= callback;
        
        public void RegisterOnEffectRemoved(Action<string> callback)
            => EffectRemoved += callback;

        public void UnregisterOnEffectRemoved(Action<string> callback)
            => EffectRemoved -= callback;
        
        public void RegisterOnEffectExpired(Action<string> callback)
            => EffectExpired += callback;

        public void UnregisterOnEffectExpired(Action<string> callback)
            => EffectExpired -= callback;

        [Server]
        private void ServerOnEffectApplied(string effectId)
        {
            var handler = EffectApplied;
            handler?.Invoke(effectId);
        }
        
        [ClientRpc]
        private void BroadcastOnEffectApplied(string effectId)
        {
            if (isServer) return;
            var handler = EffectApplied;
            handler?.Invoke(effectId);
        }

        [Server]
        private void ServerOnEffectRemoved(string effectId)
        {
            var handler = EffectRemoved;
            handler?.Invoke(effectId);
        }
        
        [ClientRpc]
        private void BroadcastOnEffectRemoved(string effectId)
        {
            if (isServer) return;
            var handler = EffectRemoved;
            handler?.Invoke(effectId);
        }

        [Server]
        private void ServerOnEffectExpired(string effectId)
        {
            var handler = EffectExpired;
            handler?.Invoke(effectId);
        }
        
        [ClientRpc]
        private void BroadcastOnEffectExpired(string effectId)
        {
            if (isServer) return;
            
            var handler = EffectExpired;
            handler?.Invoke(effectId);
        }
    }
}