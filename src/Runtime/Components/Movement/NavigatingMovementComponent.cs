using System.Linq;
using JetBrains.Annotations;
using Mirror;
using MyRpg.Core.Animation;
using MyRpg.Core.Components;
using MyRpg.Core.Events;
using MyRpg.Core.Models;
using MyRpg.Core.Modifiers;
using UnityEngine;
using UnityEngine.AI;

namespace MyRpg.Components.Movement
{
    public class NavigatingMovementComponent: NetworkBehaviour, INavigatingMovementComponent
    {
        private IStatusComponent _statusComponent;
        private ICharacterStatusProvider[] _characterStatusProviders;

        [CanBeNull] private ICharacterAnimationController _characterAnimationController;

        private NavMeshAgent _agent;
        private float _maxSpeed;

        private bool CanMove => _characterStatusProviders.All(x => x.GetCharacterStatus().CanMove);
        public bool IsMoving => !(_agent.isStopped || _agent.pathPending);

        private void Awake()
        {
            _statusComponent = GetRequiredComponent<IStatusComponent>();
            _characterStatusProviders = GetComponents<ICharacterStatusProvider>();
            _characterAnimationController = GetComponent<ICharacterAnimationController>();
            _agent = GetComponentInParent<NavMeshAgent>();

            _agent.isStopped = true;
            _agent.updateRotation = false;
        }

        [ServerCallback]
        private void Update()
        {
            if (!IsMoving) return;

            if (!CanMove)
            {
                _agent.speed = 0;
                _agent.isStopped = true;
                return;
            }

            _agent.isStopped = false;

            if (_agent.remainingDistance < 0.05f)
            {
                ServerStop();
                return;
            }

            var characterSpeed = _statusComponent.GetMovementSpeed();
            _agent.speed = Mathf.Min(characterSpeed, _maxSpeed);
            var animationSpeed = _agent.velocity.magnitude / (2 * characterSpeed);
            _characterAnimationController.ServerUpdateMovement(_agent.velocity.normalized, animationSpeed);
            
            MovementEventHandler.InvokeOnMovement(gameObject, _agent.velocity);
        }

        [Server]
        public void ServerSetTarget(Vector2 target, float maxSpeed = float.MaxValue)
        {
            _agent.isStopped = false;
            _agent.updatePosition = true;
            _agent.SetDestination(target);
            _maxSpeed = maxSpeed;
        }

        [Server]
        public void ServerStop()
        {
            _agent.isStopped = true;
            _agent.updatePosition = false;
            _characterAnimationController.ServerUpdateMovement(Vector2.zero, 0);
        }

        public void ServerInitialize(Character character)
        {
            
        }
    }
}