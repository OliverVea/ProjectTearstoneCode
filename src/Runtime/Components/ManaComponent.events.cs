using System;
using Mirror;
using MyRpg.Core.Events;

namespace MyRpg.Components
{
    public partial class ManaComponent : IManaEventHandler
    {
        private event Action<ManaChangedData> Death;

        public void RegisterOnManaChanged(Action<ManaChangedData> callback)
            => Death += callback;

        public void UnregisterOnManaChanged(Action<ManaChangedData> callback)
            => Death -= callback;

        [Server]
        private void ServerOnManaChanged(ManaChangedData data)
        {
            var handler = Death;
            handler?.Invoke(data);
        }

        [ClientRpc]
        private void BroadcastOnManaChanged(ManaChangedData data)
        {
            if (isServer) return;

            var handler = Death;
            handler?.Invoke(data);
        }
    }
}