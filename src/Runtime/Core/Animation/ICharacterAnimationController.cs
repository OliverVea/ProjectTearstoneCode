using UnityEngine;

namespace MyRpg.Core.Animation
{
    public interface ICharacterAnimationController
    {
        void ServerTriggerAttack();
        void ServerTriggerSpellCast();
        void ServerUpdateMovement(Vector2 direction, float movementSpeed);
    }
}