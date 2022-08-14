using UnityEngine;

namespace MyRpg.Core.Components
{
    public interface IProjectileMovementComponent : IBase
    {
        void SetTarget(Transform target);
        float RemainingDistance();
    }
}