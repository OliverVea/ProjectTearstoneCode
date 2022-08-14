using Mirror;
using MyRpg.Core.Components;
using MyRpg.Core.Models;
using MyRpg.Core.Modifiers;
using UnityEngine;

namespace MyRpg.Components
{
    [RequireComponent(typeof(IDeathComponent))]
    public partial class DeathComponent : NetworkBehaviour, IDeathComponent, IDeathProvider, ICharacterStatusProvider
    {
        private Animator _animator;
        private Collider2D[] _colliders;
        
        
        [field: SyncVar] public bool Dead { get; private set; }
        
        private void Awake()
        {
            _animator = GetComponentInChildren<Animator>();
            _colliders = GetComponents<Collider2D>();
            
            RegisterOnDeath(Die);
        }

        private void Start()
        {
            if (Dead) Die();
        }

        [Server]
        public void ServerDeath()
        {
            Dead = true;
            ServerOnDeath();
            BroadcastOnDeath();
        }

        private void Die()
        {
            var recipient = isClient ? "Client" : "Server";
            Debug.Log($"{recipient}: Entity {gameObject} died.");
            
            _animator.SetTrigger(Animator.StringToHash("Die"));
            
            foreach (var c in _colliders) c.enabled = false;
        }

        public void ServerInitialize(Character character)
        {
            
        }

        public CharacterStatus GetCharacterStatus()
        {
            return new CharacterStatus
            {
                CanAttack = !Dead,
                CanCast = !Dead,
                CanMove = !Dead
            };
        }
    }
}