using Mirror;
using MyRpg.Core.Components;
using MyRpg.Core.Models;
using UnityEngine;

namespace MyRpg.Components
{
    public partial class TargetingComponent : NetworkBehaviour, ITargetingComponent
    {
        [field: SerializeField] [field: SyncVar] public GameObject Target { get; private set; }
        [field: SerializeField] [field: SyncVar] public GameObject HoverTarget { get; private set; }

        [Command] 
        public void CommandSetTarget(GameObject newTarget) 
            => ServerSetTarget(newTarget);
        
        [Command] 
        public void CommandSetHoverTarget(GameObject newHoverTarget) 
            => ServerSetHoverTarget(newHoverTarget);
        
        [Server] 
        public void ServerSetTarget(GameObject newTarget)
        {
            if (newTarget == Target) return;

            var oldTarget = Target;
            Target = newTarget;
            
            ServerOnNewTarget(oldTarget, newTarget);
            BroadcastOnNewTarget(oldTarget, newTarget);
        }

        [Server]
        private void ServerSetHoverTarget(GameObject newTarget)
        {
            if (newTarget == HoverTarget) return;

            var oldTarget = HoverTarget;
            HoverTarget = newTarget;

            ServerOnNewHoverTarget(oldTarget, newTarget);
            BroadcastOnNewHoverTarget(oldTarget, newTarget);
        }

        public GameObject GetTargetsTarget()
        {
            if (Target == null) return null;
            var targetingComponent = Target.GetComponent<ITargetingComponent>();
            return targetingComponent?.Target;
        }
        
        public void ServerInitialize(Character messageCharacter) { }
    }
}