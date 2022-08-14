using JetBrains.Annotations;
using Mirror;
using MyRpg.Core.Animation;
using MyRpg.Core.Components;
using MyRpg.Core.Constants;
using MyRpg.Core.Models;
using MyRpg.Core.Modifiers;
using UnityEngine;
using UnityEngine.AI;

namespace MyRpg.Components.Movement
{
    public class ChargeComponent : NetworkBehaviour, IChargeComponent, ICharacterStatusProvider
    {
        [SyncVar] private Charge _charge;
        public bool InCharge => _charge != null;
        
        private IStatusComponent _statusComponent;
        [CanBeNull] private ICharacterAnimationController _characterAnimationController;
        [CanBeNull] private NavMeshAgent _navMeshAgent;

        private NetworkTransform _transform;

        private bool HasAuthority =>
            _transform.clientAuthority && hasAuthority || !_transform.clientAuthority && isServer;
        
        private void Awake()
        {
            _statusComponent = GetRequiredComponent<IStatusComponent>();
            _characterAnimationController = GetComponent<ICharacterAnimationController>();
            _navMeshAgent = GetComponent<NavMeshAgent>();
            _transform = GetRequiredComponent<NetworkTransform>();
        }

        private void Update()
        {
            if (!InCharge || !HasAuthority) return;
            
            var delta = _charge.target.transform.position - transform.position;
            if (delta.magnitude > _statusComponent.GetAttackRange())
            {
                _characterAnimationController?.ServerUpdateMovement(delta, 1f);
                transform.position += delta.normalized * (ConstantValues.ChargeSpeed * Time.deltaTime);
                return;
            }
            
            if (isServer) ServerCompleteCharge();
            else CommandCompleteCharge();
        }

        [Command]
        private void CommandCompleteCharge()
        {
            ServerCompleteCharge();
        }

        [Server]
        private void ServerCompleteCharge()
        {
            var charge = _charge;
            _charge = null;
            
            if (charge == null) return;

            if (_navMeshAgent != null)
            {
                _navMeshAgent.updatePosition = true;
                _navMeshAgent.isStopped = false;
            }
            
            var targetEffectsComponent = charge.target.GetComponent<IEffectsComponent>();
            targetEffectsComponent?.ServerApplyEffect(gameObject, charge.effectId);
            
            _characterAnimationController?.ServerUpdateMovement(Vector2.zero, 1f);
        }
        
        [Server]
        public void ServerSetCharge(GameObject target, string effectId)
        {
            if (InCharge) return;

            if (_navMeshAgent != null)
            {
                _navMeshAgent.updatePosition = false;
                _navMeshAgent.isStopped = true;
            }

            _charge = new Charge
            {
                effectId = effectId,
                target = target
            };
        }

        // ICharacterStatusProvider
        public CharacterStatus GetCharacterStatus()
        {
            if (!InCharge) return new CharacterStatus();

            return new CharacterStatus
            {
                CanAttack = false,
                CanCast = false,
                CanMove = false
            };
        }

        public void ServerInitialize(Character character)
        {
            
        }
    }
}