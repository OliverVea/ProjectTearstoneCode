using System;
using UnityEngine;

namespace MyRpg.Core.Models
{
    [CreateAssetMenu(menuName = "Spells/Create Effect")]
    public class Effect : ScriptableObject
    {
        [field: SerializeField] public string Id { get; private set; } = Guid.NewGuid().ToString();
        [field: SerializeField] public EffectType EffectType { get; private set; }
        [field: SerializeField] public float Duration  { get; private set; } = float.MaxValue;
        [field: SerializeField] public GameObject EffectOnImpact { get; private set; }
        [field: SerializeField] public Sprite Icon { get; private set; }
        [field: SerializeField] public string[] BlocksSpells { get; private set; } = Array.Empty<string>();
        [field: SerializeField] public bool IsHidden { get; private set; }
        [field: SerializeField] public string EffectName { get; set; }
        [field: SerializeField] public string Description { get; set; }

        // Damage
        [field: SerializeField] public float DamageOnApplication { get; private set; }
        [field: SerializeField] public float DamagePerSecond { get; private set; }
        [field: SerializeField] public float DamageOnExpiry { get; private set; }
        [field: SerializeField] public float DamageOnRemoval { get; private set; }
        
        // Healing
        [field: SerializeField] public float HealingOnApplication { get; private set; }
        [field: SerializeField] public float HealingPerSecond { get; private set; }
        [field: SerializeField] public float HealingOnExpiry { get; private set; }
        [field: SerializeField] public float HealingOnRemoval { get; private set; }

        // Movement speed
        [field: SerializeField] public float Slow { get; private set; }
        [field: SerializeField] public float SpeedBoost { get; private set; }
        [field: SerializeField] public float LowerSpeedLimit { get; private set; }

        // Shield
        [field: SerializeField] public float Absorption { get; private set; }
        
        // Statuses
        [field: SerializeField] public bool SetDead { get; private set; }
        [field: SerializeField] public bool SetFrozen { get; private set; }
        [field: SerializeField] public bool SetChilled { get; private set; }
        [field: SerializeField] public bool SetStunned { get; private set; }
        [field: SerializeField] public bool SetSilenced { get; private set; }
        [field: SerializeField] public bool Interrupts { get; private set; }
        [field: SerializeField] public bool Taunts { get; private set; }
        [field: SerializeField] public float Lifesteal { get; private set; }
        [field: SerializeField] public bool FocusesCaster { get; private set; }
        
        [field: SerializeField] public bool RemoveOnDeath  { get; private set; } = true;
        [field: SerializeField] public bool RemoveOnDamageTaken { get; private set; }
        [field: SerializeField] public bool RemoveWhenShieldDown { get; private set; }
    }
}