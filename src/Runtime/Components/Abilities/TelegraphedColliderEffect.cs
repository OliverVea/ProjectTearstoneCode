using System.Collections.Generic;
using System.Linq;
using Mirror;
using MyRpg.Core.Components;
using MyRpg.Core.Constants;
using MyRpg.Core.Models;
using UnityEngine;

namespace MyRpg.Components.Abilities
{
    public class TelegraphedColliderEffect : NetworkBehaviour
    {
        private NetworkAnimator _animator;
        private Collider2D _collider;

        [SerializeField] private float telegraphSeconds;
        [SerializeField] private Effect effectOnHit;
        [SerializeField] private Faction[] canHitFactions;

        private float age;
        private bool telegraphed;

        private void Awake()
        {
            _animator = GetComponent<NetworkAnimator>();
            _collider = GetComponent<Collider2D>();
        }

        [ServerCallback]
        private void Update()
        {
            if (telegraphed) return;
            
            age += Time.deltaTime;

            if (age < telegraphSeconds) return;
            
            _animator.SetTrigger("Strike");
            telegraphed = true;
        }

        public void Hit()
        {
            if (!isServer) return;

            var hitColliders = new List<Collider2D>();
            _collider.OverlapCollider(new ContactFilter2D(), hitColliders);

            if (!hitColliders.Any()) return;
            foreach (var hitCollider in hitColliders) HitCollider(hitCollider);
        }

        private void HitCollider(Collider2D hitCollider)
        {
            var statusComponent = hitCollider.GetComponent<IStatusComponent>();
            if (statusComponent == null) return;
            
            if (!canHitFactions.Contains(statusComponent.GetFaction())) return;

            var effectsComponent = hitCollider.GetComponent<IEffectsComponent>();
            effectsComponent.ServerApplyEffect(null, effectOnHit.Id);
        } 

        public void Destroy()
        {
            if (!isServer) return;
            
            NetworkServer.Destroy(gameObject);
        }
    }
}