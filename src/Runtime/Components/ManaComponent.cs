using System;
using Mirror;
using MyRpg.Core.Components;
using MyRpg.Core.Events;
using MyRpg.Core.Models;
using MyRpg.Core.StaticProviders;
using Unity.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

namespace MyRpg.Components
{
    public partial class ManaComponent : NetworkBehaviour, IManaComponent
    {
        [SerializeField] [ReadOnly] [SyncVar] private float currentMana;
        [SerializeField] [ReadOnly] [SyncVar] private float maxMana;
        [SerializeField] [ReadOnly] [SyncVar] private float manaPerSecond;
        [SerializeField] private float _secondsBetweenUpdates = 0.1f;
        
        private IStatusComponent _statusComponent;

        private float _lastUpdate = 100f;

        public float GetCurrentMana() => currentMana;
        public float GetMaxMana() => maxMana;

        private void Awake()
        {
            _statusComponent = GetRequiredComponent<IStatusComponent>();
        }
        
        [ServerCallback]
        private void Update()
        {
            if (maxMana <= 0) return;
            
            _lastUpdate += Time.deltaTime;
            if (_lastUpdate <= _secondsBetweenUpdates) return;
            
            var combatModifier = CombatProvider.IsInCombat(gameObject) ? 0.2f : 1f;
            
            ChangeMana(manaPerSecond * _lastUpdate * combatModifier);
            
            _lastUpdate = -_secondsBetweenUpdates * Random.value;
        }

        [Server]
        public void ServerSpendMana(float manaCost)
        {
            if (currentMana < manaCost) throw new Exception("Tried to spent mana when currentMana is too low.");
            ChangeMana(-manaCost);
        }

        private void ChangeMana(float change)
        {
            currentMana = Mathf.Clamp(currentMana + change, 0, maxMana);

            var data = new ManaChangedData
            {
                NewMana = currentMana,
                Change = change,
                MaxMana = maxMana
            };
            
            ServerOnManaChanged(data);
            BroadcastOnManaChanged(data);
        }

        public void ServerInitialize(Character character)
        {
            var baseMana = _statusComponent.GetManaForClass(character.characterClass);
            
            currentMana = baseMana;
            maxMana = baseMana;
        }
    }
}