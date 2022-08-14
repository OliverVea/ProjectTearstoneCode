using UnityEngine;

namespace MyRpg.Core.Components
{
    public interface INavigatingMovementComponent : IBase
    {
        bool IsMoving { get; }
        void ServerSetTarget(Vector2 target, float maxSpeed = float.MaxValue);
        void ServerStop();
    }
}