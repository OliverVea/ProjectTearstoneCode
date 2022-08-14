using System.Collections.Generic;

namespace MyRpg.Core.Constants
{
    public enum Faction
    {
        Player,
        Hostile
    }

    public static class FactionRelations
    {
        private static readonly Dictionary<Faction, List<Faction>> HostileFactions = new Dictionary<Faction, List<Faction>>
        {
            { Faction.Player, new List<Faction> { Faction.Hostile } },
            { Faction.Hostile, new List<Faction> { Faction.Player } }
        };

        public static bool IsHostile(Faction ownFaction, Faction targetFaction)
        {
            if (!FactionRelations.HostileFactions.TryGetValue(ownFaction, out var hostileFactions)) 
                return false;
            return hostileFactions.Contains(targetFaction);
        }

        public static bool IsFriendly(Faction ownFaction, Faction targetFaction)
        {
            return ownFaction == targetFaction;
        }

        public static bool IsNeutral(Faction ownFaction, Faction targetFaction)
        {
            return !IsFriendly(ownFaction, targetFaction) && !IsHostile(ownFaction, targetFaction);
        }

        public static bool IsAttackable(Faction ownFaction, Faction targetFaction)
        {
            return !IsFriendly(ownFaction, targetFaction);
        }
    }
}