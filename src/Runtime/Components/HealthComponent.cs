using System;
using Mirror;
using MyRpg.Core.Components;
using MyRpg.Core.Events;
using MyRpg.Core.Models;
using MyRpg.Core.Modifiers;
using UnityEngine;

namespace MyRpg.Components
{
    [RequireComponent(typeof(IDeathComponent))]
    public partial class HealthComponent : NetworkBehaviour, IHealthComponent
    {
        [field: SerializeField] [field: SyncVar] public float currentHealth { private get; [Server] set; }

        [field: SerializeField] [field: SyncVar] public float maxHealth { private get; [Server] set; }

        private IDeathComponent _deathComponent;
        private IStatusComponent _statusComponent;
        
        private IAbsorptionProvider[] _absorptionProviders;

        public float GetMaxHealth() => maxHealth;
        public float GetMissingHealth() => maxHealth - currentHealth;
        public float GetCurrentHealth() => currentHealth;
        private bool HasFullHealth()  => Math.Abs(currentHealth - maxHealth) < float.Epsilon;
        private bool HasNoHealth() => currentHealth == 0;
        
        private void Awake()
        {
            _deathComponent = GetRequiredComponent<IDeathComponent>();
            _statusComponent = GetRequiredComponent<IStatusComponent>();
            
            _absorptionProviders = GetComponents<IAbsorptionProvider>();
            
        }

        [Server]
        public void ServerTakeDamage(GameObject source, float damage)
        {
            if (HasNoHealth()) return;
            
            damage = ServerAbsorbDamage(damage);

            damage *= Mathf.Clamp01(1 - _statusComponent.GetDamageReduction());
            ChangeHealth(source, -damage);
            if (HasNoHealth()) _deathComponent?.ServerDeath();
        }

        [Server]
        private float ServerAbsorbDamage(float damage)
        {
            foreach (var absorptionProvider in _absorptionProviders)
            {
                damage = absorptionProvider.ServerAbsorbDamage(damage);
                if (damage <= 0) return 0f;
            }

            return damage;
        }

        [Server]
        public void ServerTakeHealing(GameObject source, float healing)
        {
            if (HasFullHealth()) return;
            ChangeHealth(source, healing);
        }


        private void ChangeHealth(GameObject source, float change)
        {
            if (change == 0 || change < 0 && HasNoHealth() || change > 0 && HasFullHealth()) return;
            
            currentHealth = Mathf.Clamp(currentHealth + change, 0, maxHealth);

            var data = new HealthChangedData
            {
                Source = source,
                Change = change,
                MaxHealth = maxHealth,
                NewHealth = currentHealth
            };
            
            ServerOnHealthChanged(data);
            BroadcastOnHealthChanged(data);
        }

        public void ServerInitialize(Character character)
        {
            var baseHealth = _statusComponent.GetHealthForClass(character.characterClass);
            
            currentHealth = baseHealth;
            maxHealth = baseHealth;
        }
    }
}