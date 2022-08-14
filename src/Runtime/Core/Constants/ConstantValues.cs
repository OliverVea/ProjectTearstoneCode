using UnityEngine;

namespace MyRpg.Core.Constants
{
    public static class ConstantValues
    {
        public const float MaxMovementModifier = 2.2f;
        public const string DefaultCharacterName = "Unknown";
        public const float ChilledMovementReduction = 0.4f;
        
        public const float DefaultSpellRange = 14f;
        public const float DefaultAggroRange = 10f;
        public const float DefaultProjectileSpeed = 15f;
        
        public const float ThreatPerDamage = 1f;
        public const float ThreatPerHealing = 1f;
        public const float ThreatOnPull = 20f;
        
        public const float ServerProjectileHitRange = 0.1f;
        public const float ClientProjectileHitRange = 0.25f;
        public const float ChargeSpeed = 12f;
        public const float MeleeAttackRange = 1.8f;
        
        public const float LowThreshold = 0.25f;
        public const float HighThreshold = 0.75f;
        public const float ThreatOnTaunt = 100f;
        public const float ThreatPropagationRange = 5f;
        
        public const KeyCode UseKey = KeyCode.E;
        public const KeyCode ResetKey = KeyCode.F12;

        public static class PrefKeys
        {
            public const string SelectedCharacter = "SelectedCharacter";
        }

    }
}