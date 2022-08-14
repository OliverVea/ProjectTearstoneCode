using System.Collections.Generic;
using MyRpg.Core.Components;
using MyRpg.Core.Constants;
using UnityEngine;

namespace MyRpg.Core.StaticProviders
{
    public static class FactionRelationsProvider
    {
        private static readonly Dictionary<(Transform, Transform), bool> AttackableCache = new Dictionary<(Transform, Transform), bool>();
        private static readonly Dictionary<(Transform, Transform), bool> FriendlyCache = new Dictionary<(Transform, Transform), bool>();

        public static bool IsAttackable(Transform attacker, Transform target)
        {
            var key = GetKey(attacker, target);
            if (AttackableCache.ContainsKey(key)) return AttackableCache[key];

            var attackerStatus = attacker.GetComponent<IStatusComponent>();
            var targetStatus = target.GetComponent<IStatusComponent>();

            var value = FactionRelations.IsAttackable(attackerStatus.GetFaction(), targetStatus.GetFaction());
            AttackableCache[key] = value;

            return value;
        }

        public static bool IsFriendly(Transform attacker, Transform target)
        {
            var key = GetKey(attacker, target);
            if (FriendlyCache.ContainsKey(key)) return FriendlyCache[key];

            var attackerStatus = attacker.GetComponent<IStatusComponent>();
            var targetStatus = target.GetComponent<IStatusComponent>();

            var value = FactionRelations.IsFriendly(attackerStatus.GetFaction(), targetStatus.GetFaction());
            FriendlyCache[key] = value;

            return value;
            
        }
        
        private static (Transform, Transform) GetKey(Transform attacker, Transform target)
            => (attacker, target);
    }
}