using MyRpg.Core.Components;
using UnityEngine;

namespace MyRpg.Animation
{
    public class PlayerAnimationTriggers : MonoBehaviour
    {
        private IAttackingComponent _attackingComponent;

        private void Awake()
        {
            _attackingComponent = GetComponentInParent<IAttackingComponent>();
        }

        // Animation event
        void OnHit()
        {
            _attackingComponent.OnHit();
        }
    }
}