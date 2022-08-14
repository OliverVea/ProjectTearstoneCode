using System;
using Mirror;
using MyRpg.Core.Components;
using MyRpg.Core.Models;
using MyRpg.Core.Network;
using UnityEngine;

namespace MyRpg.Components.Abilities
{
    public class InteractableEffectComponent : NetworkBehaviour, IInteractableComponent
    {
        private enum Target
        {
            Self,
            AllPlayers
        }
    
        [SerializeField] private Effect effect;
        [SerializeField] private Target target;

        private IEffectsComponent _effectsComponent;
        private IMyNetworkManager _networkManager;

        private void Awake()
        {
            if (target == Target.Self) 
                _effectsComponent = GetRequiredComponent<IEffectsComponent>();
            
            if (target == Target.AllPlayers)
                _networkManager = GameObject.FindWithTag("NetworkManager").GetComponent<IMyNetworkManager>();
        }

        public void ServerInteract()
        {
            switch (target)
            {
                case Target.Self:
                    _effectsComponent.ServerApplyEffect(gameObject, effect.Id);
                    Debug.Log($"Applied effect {effect.EffectName} to boss.");
                    return;
                case Target.AllPlayers:
                    foreach (var player in  PartyLookup.GetParty())
                        player.GetComponent<IEffectsComponent>().ServerApplyEffect(gameObject, effect.Id);
                    Debug.Log($"Applied effect {effect.EffectName} to players.");
                    return;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            
        }
    }
}