using System;
using Mirror;
using MyRpg.Core.Events;
using UnityEngine;

namespace MyRpg.Components
{
    public partial class HealthComponent : IHealthEventHandler
    {
        private static event Action<GameObject, HealthChangedData> AllHealthChanged;
        private event Action<HealthChangedData> HealthChanged;

        public void RegisterOnHealthChanged(Action<HealthChangedData> callback)
            => HealthChanged += callback;

        public void UnregisterOnHealthChanged(Action<HealthChangedData> callback)
            => HealthChanged -= callback;

        public void RegisterOnAllHealthChanged(Action<GameObject, HealthChangedData> callback)
            => AllHealthChanged += callback;

        public void UnregisterOnAllHealthChanged(Action<GameObject, HealthChangedData> callback)
            => AllHealthChanged -= callback;

        [Server]
        private void ServerOnHealthChanged(HealthChangedData data)
            => OnHealthChanged(data);

        [ClientRpc]
        private void BroadcastOnHealthChanged(HealthChangedData data)
        {
            if (isServer) return;
            OnHealthChanged(data);
        }

        private void OnHealthChanged(HealthChangedData data)
        {
            var handler = HealthChanged;
            handler?.Invoke(data);

            var allHandler = AllHealthChanged;
            allHandler?.Invoke(gameObject, data);
            
        }
    }
}