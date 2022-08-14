using System;
using System.Linq;
using Mirror;
using MyRpg.Core.Attributes;
using MyRpg.Core.Components;
using MyRpg.Core.Models;
using MyRpg.Core.Network;
using UnityEngine;

namespace MyRpg.Components.Abilities
{
    public class PylonComponent : NetworkBehaviour
    {
        private PylonAbility ability;
        [SerializeField] [ReadOnly] private Effect keyEffect;
        
        [SerializeField] private NetworkAnimator animator;
        [SerializeField] private SpriteRenderer spriteRenderer;
        [SerializeField] private float radius;
        
        private IMyNetworkManager _networkManager;

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, radius);
        }

        private void Awake()
        {
            _networkManager = GameObject.FindWithTag("NetworkManager").GetComponent<IMyNetworkManager>();
        }

        [Server]
        public void ServerStartWaiting(PylonAbility newAbility, Color color)
        {
            if (ability != null) return;
            
            animator.SetTrigger("Start");
            spriteRenderer.color = color;
            BroadcastColorChange(color);

            ability = newAbility;
        }

        [ClientRpc]
        private void BroadcastColorChange(Color newColor)
        {
            spriteRenderer.color = newColor;
        }

        [Server]
        public void ServerFinishWaiting()
        {
            if (ability == null) return;

            animator.SetTrigger("Finish");

            var player = GetNearbyPlayerWithKeyEffect();
            if (player == null)
            {
                TriggerPylonAbility();
            }
            else
            {
                player.GetComponent<IEffectsComponent>().ServerRemoveEffect(keyEffect.Id);
            }

            ability = null;
        }

        private void TriggerPylonAbility()
        {
            ability.Interact.GetComponent<IInteractableComponent>().ServerInteract();
        }

        private GameObject GetNearbyPlayerWithKeyEffect()
        {
            var players =  PartyLookup.GetParty();
            return players.SingleOrDefault(x 
                => Vector2.Distance(x.transform.position, transform.position) < radius);
        }

        public void SetKeyEffect(Effect effect)
        {
            keyEffect = effect;
        }
    }
}