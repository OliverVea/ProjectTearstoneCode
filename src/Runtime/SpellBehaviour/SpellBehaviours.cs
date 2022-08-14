using System.Collections.Generic;
using MyRpg.Core;

namespace MyRpg.SpellBehaviour
{
    public static class SpellBehaviours
    {
        private static readonly Dictionary<string, ISpellBehaviour> Behaviours = new Dictionary<string, ISpellBehaviour>
        {
            // Mage
            {"fireball", new FireballBehaviour()},
            {"frostbolt", new FireballBehaviour()},
            
            // Priest
            {"renew", new RenewBehaviour()},
            {"lesserheal", new LesserHealBehaviour()},
            
            // Warrior
            {"charge", new ChargeBehaviour()},
            {"shieldbash", new ShieldBashBehaviour()},
            
            // Enemy-only
            {"deathstrike", new DeathStrikeBehaviour()}
        };

        public static ISpellBehaviour GetBehaviour(string spellId)
        {
            Behaviours.TryGetValue(spellId, out var spellBehaviour);
            return spellBehaviour;
        }
    }
}