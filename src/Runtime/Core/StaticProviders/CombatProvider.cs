using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MyRpg.Core.StaticProviders
{
    public static class CombatProvider
    {
        private static readonly List<ICombatProvider> CombatProviders = new List<ICombatProvider>();
        
        public static bool IsInCombat(GameObject character)
        {
            return CombatProviders.Any(x => x.IsInCombat(character));
        }

        public static void RegisterProvider(ICombatProvider provider)
        {
            if (CombatProviders.Contains(provider)) return;
            CombatProviders.Add(provider);
        }

        public static void UnregisterProvider(ICombatProvider provider)
        {
            if (!CombatProviders.Contains(provider)) return;
            CombatProviders.Remove(provider);
        }
    }
}