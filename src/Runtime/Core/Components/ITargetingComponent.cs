using UnityEngine;

namespace MyRpg.Core.Components
{
    public interface ITargetingComponent : IBase
    {
        GameObject Target { get; }
        GameObject HoverTarget { get; }

        void CommandSetTarget(GameObject newTarget);
        void CommandSetHoverTarget(GameObject newTarget);
        void ServerSetTarget(GameObject newTarget);
        GameObject GetTargetsTarget();
    }
}