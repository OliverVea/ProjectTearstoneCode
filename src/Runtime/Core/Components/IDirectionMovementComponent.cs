using UnityEngine;

namespace MyRpg.Core.Components
{
    public interface IDirectionMovementComponent : IBase
    {
        void SetDirection(Vector2 direction);
        void StopMovement();
    }
}