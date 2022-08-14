using System;
using Mirror;
using MyRpg.Core.Events;
using UnityEngine;

namespace MyRpg.Components
{
    public partial class TargetingComponent : ITargetingEventHandler
    {
        private event Action<GameObject, GameObject> NewTarget;
        private event Action<GameObject, GameObject> NewHoverTarget;
            
        public void RegisterOnNewTarget(Action<GameObject, GameObject> callback)
            => NewTarget += callback;
        public void UnregisterOnNewTarget(Action<GameObject, GameObject> callback)
            => NewTarget -= callback;
            
        public void RegisterOnNewHoverTarget(Action<GameObject, GameObject> callback)
            => NewHoverTarget += callback;
        public void UnregisterOnNewHoverTarget(Action<GameObject, GameObject> callback)
            => NewHoverTarget -= callback;

        [Server]
        private void ServerOnNewTarget(GameObject oldTarget, GameObject newTarget)
        {
            var handler = NewTarget;
            handler?.Invoke(oldTarget, newTarget);
        }

        [Server]
        private void ServerOnNewHoverTarget(GameObject oldTarget, GameObject newTarget)
        {
            var handler = NewHoverTarget;
            handler?.Invoke(oldTarget, newTarget);
        }

        [ClientRpc]
        private void BroadcastOnNewTarget(GameObject oldTarget, GameObject newTarget)
        {
            if (isServer) return;

            var handler = NewTarget;
            handler?.Invoke(oldTarget, newTarget);
        }

        [ClientRpc]
        private void BroadcastOnNewHoverTarget(GameObject oldTarget, GameObject newTarget)
        {
            if (isServer) return;

            var handler = NewHoverTarget;
            handler?.Invoke(oldTarget, newTarget);
        }
        
    }
}