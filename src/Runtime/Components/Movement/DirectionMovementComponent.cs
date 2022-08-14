using System;
using System.Linq;
using JetBrains.Annotations;
using Mirror;
using MyRpg.Core.Animation;
using MyRpg.Core.Components;
using MyRpg.Core.Events;
using MyRpg.Core.Models;
using MyRpg.Core.Modifiers;
using UnityEngine;

namespace MyRpg.Components.Movement
{
    public class DirectionMovementComponent : NetworkBehaviour, IDirectionMovementComponent
    {
        private IStatusComponent _statusComponent;
        private ICharacterStatusProvider[] _characterStatusProviders;
        [CanBeNull] private ICharacterAnimationController _characterAnimationController;

        private Vector2 _direction = Vector2.zero;
        private IDeathEventHandler _deathEventHandler;

        private bool CanMove => _characterStatusProviders.All(x => x.GetCharacterStatus().CanMove);

        private void Awake()
        {
            _statusComponent = GetRequiredComponent<IStatusComponent>();
            _characterStatusProviders = GetComponents<ICharacterStatusProvider>();
            _characterAnimationController = GetComponent<ICharacterAnimationController>();

            _deathEventHandler = GetComponent<IDeathEventHandler>();

            if (GetComponent<INavigatingMovementComponent>() != null)
                Debug.LogError( "IDirectionMovementComponent and IDestinationMovementComponent are incompatible.");
        }

        private void OnEnable()
        {
            _deathEventHandler.RegisterOnDeath(OnDeath);
        }

        private void OnDisable()
        {
            _deathEventHandler.UnregisterOnDeath(OnDeath);
        }

        private void OnDeath()
        {
            _direction = Vector2.zero;
        }

        [ClientCallback]
        private void Update()
        {
            if (!hasAuthority || !CanMove) return;

            var velocity = _direction * _statusComponent.GetMovementSpeed();
            transform.position += new Vector3(velocity.x, velocity.y, 0) * Time.deltaTime;
            
            CommandUpdateAnimation(velocity);
        }

        [Command]
        private void CommandUpdateAnimation(Vector2 velocity)
        {
            _characterAnimationController?.ServerUpdateMovement(velocity, 1f);
        }

        [Client]
        public void SetDirection(Vector2 direction)
        {
            if (!hasAuthority)
            {
                Debug.LogWarning($"Client without authority tried to set movement direction of {gameObject}.");
                return;
            }
            
            _direction = direction.normalized;
            CommandOnMovement(_direction);
        }

        [Command]
        private void CommandOnMovement(Vector2 direction)
        {
            MovementEventHandler.InvokeOnMovement(gameObject, direction);
            BroadcastOnMovement(direction);
        }

        private void BroadcastOnMovement(Vector2 direction)
        {
            if (isServer) return;
            MovementEventHandler.InvokeOnMovement(gameObject, direction);
        }

        [Client]
        public void StopMovement()
        {
            if (!hasAuthority)
            {
                Debug.LogWarning($"Client without authority tried to stop movement of {gameObject}.");
                return;
            }

            _direction = Vector2.zero;
        }

        public void ServerInitialize(Character character)
        {
            
        }
    }
}