using Mirror;
using MyRpg.Core.Animation;
using MyRpg.Core.Components;
using MyRpg.Core.Helpers;
using UnityEngine;

namespace MyRpg.Animation
{
    public class CharacterAnimationController : NetworkBehaviour, ICharacterAnimationController
    {
        [SerializeField] private GameObject unitRoot;
        
        private static readonly int NormalState = Animator.StringToHash("NormalState");
        private static readonly int RunState = Animator.StringToHash("RunState");
        private static readonly int Attack = Animator.StringToHash("Attack");

        private Animator _animator;
        private IEffectsComponent _effectsComponent;

        private readonly BoolHelper _stunnedBoolHelper = new BoolHelper();
        
        private void Awake()
        {
            _animator = GetComponentInChildren<Animator>();
            _effectsComponent = GetComponent<IEffectsComponent>();
        }

        private void Update()
        {
            var isStunned = _effectsComponent.IsStunned();
            _stunnedBoolHelper.Update(isStunned);
            if (isStunned) _animator.SetFloat(RunState, 1);
            if (_stunnedBoolHelper.NowFalse) _animator.SetFloat(RunState, 0);
        }

        [Server]
        public void ServerTriggerAttack()
        {
            TriggerAttack();
            BroadCastTriggerAttack();
        }

        private void TriggerAttack()
        {
            _animator.SetFloat(NormalState, 0);
            _animator.SetTrigger(Attack);
        }

        [ClientRpc]
        private void BroadCastTriggerAttack()
        {
            if (!isServer) TriggerAttack();
        }
        
        [Server]
        public void ServerTriggerSpellCast()
        {
            TriggerSpellCast();
            BroadCastTriggerSpellCast();
        }

        private void TriggerSpellCast()
        {
            _animator.SetFloat(NormalState, 0);
            _animator.SetTrigger(Attack);
        }

        [ClientRpc]
        private void BroadCastTriggerSpellCast()
        {
            if (!isServer) TriggerSpellCast();
        }

        [Server]
        public void ServerUpdateMovement(Vector2 direction, float movementSpeed)
        {
            BroadcastUpdateMovement(direction, movementSpeed);
            UpdateRunState(direction, movementSpeed);
            UpdateSpriteDirection(direction);
        }

        [ClientRpc]
        private void BroadcastUpdateMovement(Vector2 direction, float movementSpeed)
        {
            if (!isClientOnly || !isActiveAndEnabled) return;
            UpdateRunState(direction, movementSpeed);
            UpdateSpriteDirection(direction);
        }

        private void UpdateRunState(Vector2 direction, float movementSpeed)
        {
            var animationSpeed = Mathf.Min(movementSpeed * direction.magnitude, 0.5f); // above 0.5f is drunk.
            _animator.SetFloat(RunState, animationSpeed);
        }

        private void UpdateSpriteDirection(Vector2 direction)
        {
            var currentScale = unitRoot.transform.localScale;
            var currentXScale = currentScale.x;

            var newXScale = currentXScale;
            if (direction.x > 0) newXScale = -Mathf.Abs(currentXScale);
            if (direction.x < 0) newXScale = Mathf.Abs(currentXScale);
            
            if (newXScale != currentXScale) 
                unitRoot.transform.localScale = new Vector3(newXScale, currentScale.y, currentScale.z);
        }
    }
}