using Mirror;
using MyRpg.Core.Components;
using MyRpg.Core.Constants;
using MyRpg.Core.Models;
using UnityEngine;

namespace MyRpg.Components.Movement
{
    public class ProjectileMovementComponent : NetworkBehaviour, IProjectileMovementComponent
    {
        private bool _targetHasBeenSet;
        private Transform _target;
        private const float Speed = ConstantValues.DefaultProjectileSpeed;

        private Vector3 TargetPosition => _target.position + Vector3.up * 1f;
        
        private void Update()
        {
            if (!_targetHasBeenSet) return;
            
            var delta = (TargetPosition - transform.position).normalized;
            var velocity = delta * Speed;
            
            transform.right = delta;

            if (delta.magnitude > velocity.magnitude) transform.position = TargetPosition;
            else transform.position += velocity * Time.deltaTime;
        }

        public void SetTarget(Transform target)
        {
            _target = target;
            _targetHasBeenSet = _target != null;
        }

        public float RemainingDistance()
        {
            if (!_targetHasBeenSet) return float.MaxValue;
            return (TargetPosition - transform.position).magnitude;
        }

        public void ServerInitialize(Character character)
        {
            
        }
    }
}