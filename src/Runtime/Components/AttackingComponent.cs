using System.Collections.Generic;
using JetBrains.Annotations;
using Mirror;
using MyRpg.Core.Animation;
using MyRpg.Core.Components;
using MyRpg.Core.Constants;
using MyRpg.Core.Events;
using MyRpg.Core.Models;
using UnityEngine;

namespace MyRpg.Components
{
    [RequireComponent(typeof(ITargetingComponent))]
    public class AttackingComponent : NetworkBehaviour, IAttackingComponent, IHitAnimationReceiver
    {
        [SerializeField] private float weaponDamage = 10f;
        [SerializeField] private float secondsBetweenAttacks = 1.5f;
        [SerializeField] private float attackRange = 2.0f;
        [SerializeField] private List<Faction> hostileFactions = new List<Faction>();
        
        [field: SerializeField] public bool IsAttacking { get; set; }
        private float _timeSinceLastAttack;

        private ITargetingEventHandler _targetingEventHandler;

        private IStatusComponent _statusComponent;
        private ITargetingComponent _targetingComponent;
        private IProgressComponent _progressComponent;
        [CanBeNull] private ICharacterAnimationController _characterAnimationController;

        private void Awake()
        {
            _targetingEventHandler = GetRequiredComponent<ITargetingEventHandler>();
            
            _statusComponent = GetRequiredComponent<IStatusComponent>();
            _targetingComponent = GetRequiredComponent<ITargetingComponent>();
            _progressComponent = GetRequiredComponent<IProgressComponent>();
            
            _characterAnimationController = GetComponent<ICharacterAnimationController>();
        }

        private void OnEnable()
        {

            _targetingEventHandler.RegisterOnNewTarget(OnTargetChanged);
        }

        private void OnDisable()
        {
            _targetingEventHandler.UnregisterOnNewTarget(OnTargetChanged);
        }
        
        private void Update()
        {
            _timeSinceLastAttack += Time.deltaTime;

            if (!IsAttacking || !CanAttack() || !IsInRange()) return;
            if (!(_timeSinceLastAttack >= secondsBetweenAttacks)) return;
            
            if (isServer) ServerExecuteAttackAnimation();
            else CommandExecuteAttackAnimation();
            _timeSinceLastAttack = 0;
        }
        
        private static bool TargetExists(GameObject target) => target != null;
        
        private static bool TargetIsAttackable(IStatusComponent targetStatus, List<Faction> hostileFactions) 
            => !targetStatus.IsDead() &&
               hostileFactions.Contains(targetStatus.GetFaction());
        
        private static bool IsInRange(Transform targetTransform, Transform playerTransform, float attackRange)
            => (targetTransform.position - playerTransform.position).magnitude <= attackRange;


        private static bool CanAttack(IStatusComponent statusComponent, IProgressComponent progressComponent,
            GameObject target, List<Faction> hostileFactions)
        {
            return !statusComponent.IsDead() &&
                   !progressComponent.IsCasting() && 
                   TargetExists(target) && 
                   TargetIsAttackable(target.GetComponent<IStatusComponent>(), hostileFactions);
        }
        
        private bool CanAttack() => CanAttack(_statusComponent, _progressComponent, _targetingComponent.Target, hostileFactions);
        
        public bool IsInRange() => IsInRange(_targetingComponent.Target.transform, transform, attackRange);
        
        public void OnHit()
        {
            if (!isServer || !CanAttack()) return;

            var targetHealth = _targetingComponent.Target.GetComponent<IHealthComponent>();
            targetHealth.ServerTakeDamage(gameObject, weaponDamage);
        }

        [Server]
        private void ServerExecuteAttackAnimation()
            => _characterAnimationController.ServerTriggerAttack();        

        [Command]
        private void CommandExecuteAttackAnimation()
            => ServerExecuteAttackAnimation();

        private void OnTargetChanged(GameObject oldTarget, GameObject newTarget)
        {
            IsAttacking &= TargetExists(newTarget) && 
                           TargetIsAttackable(newTarget.GetComponent<IStatusComponent>(), hostileFactions);
        }

        public void ServerInitialize(Character character)
        {
            
        }
    }
}