using System;
using Mirror;
using MyRpg.Core.Components;
using MyRpg.Core.Events;

namespace MyRpg.Components
{
    public partial class DeathComponent : IDeathEventHandler
    {
        private static event Action<IDeathComponent> AllDeath;
        private event Action Death;

        public void RegisterOnDeath(Action callback)
            => Death += callback;
        public void UnregisterOnDeath(Action callback)
            => Death -= callback;

        public void RegisterAllOnDeath(Action<IDeathComponent> callback)
            => AllDeath += callback;

        public void UnregisterAllOnDeath(Action<IDeathComponent> callback)
            => AllDeath -= callback;

        [Server]
        private void ServerOnDeath()
        {
            OnDeath();
        }

        [ClientRpc]
        private void BroadcastOnDeath()
        {
            if (isServer) return;
            OnDeath();
        }

        private void OnDeath()
        {
            var handler = Death;
            handler?.Invoke();

            var allHandler = AllDeath;
            allHandler?.Invoke(this);
        }
    }
}