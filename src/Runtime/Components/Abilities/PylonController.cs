using System;
using System.Linq;
using Mirror;
using MyRpg.Core.Attributes;
using MyRpg.Core.Components;
using MyRpg.Core.Models;
using MyRpg.Core.Network;
using UnityEngine;
using Random = UnityEngine.Random;

namespace MyRpg.Components.Abilities
{
    public class PylonController : NetworkBehaviour, IInteractableComponent
    {
        [SerializeField] private PylonComponent[] pylons;
        [SerializeField] [ReadOnly] private bool awaitingCompletion;

        [SerializeField] private PylonEffect[] effects;
        [SerializeField] private Effect keyEffect;
        [SerializeField] private int difficulty; // 0 - Normal, 1 - HC, 2 - Mythic
        private IMyNetworkManager _networkManager;

        [Serializable]
        private class PylonEffect
        {
            public PylonAbility Ability;
            public Color Color;
        }
        
        private int PartySize => PartyLookup.GetParty().Count();
        private int ActivatePylonCount => Mathf.Min(PartySize, pylons.Length, effects.Length);
        private int ApplyKeyCount => Mathf.Max(ActivatePylonCount - difficulty, 0);

        private void Awake()
        {
            _networkManager = GameObject.FindWithTag("NetworkManager").GetComponent<IMyNetworkManager>();
        }

        private void Start()
        {
            foreach (var pylon in pylons) pylon.SetKeyEffect(keyEffect);
        }

        public void ServerInteract()
        {
            if (awaitingCompletion) FinishWaiting();
            else StartWaiting();

            awaitingCompletion = !awaitingCompletion;
        }

        private void StartWaiting()
        {
            ActivatePylons();
            ApplyKeyEffects();
        }

        private void ActivatePylons()
        {
            var randomizedPylons = pylons.OrderBy(x => Random.value).ToArray();
            var randomizedEffects = effects.OrderBy(x => Random.value).ToArray();

            for (var i = 0; i < ActivatePylonCount; i++)
            {
                var pylon = randomizedPylons[i];
                var effect = randomizedEffects[i];

                pylon.ServerStartWaiting(effect.Ability, effect.Color);
            }
        }

        private void ApplyKeyEffects()
        {
            var randomizedPlayers = PartyLookup.GetParty().OrderBy(x => Random.value).ToArray();

            for (var i = 0; i < ApplyKeyCount; i++)
            {
                var player = randomizedPlayers[i];
                var playerEffectComponent = player.GetComponent<IEffectsComponent>();
                playerEffectComponent.ServerApplyEffect(null, keyEffect.Id);
            }
        }

        private void FinishWaiting()
        {
            foreach (var pylon in pylons) pylon.ServerFinishWaiting();
        }
    }
}