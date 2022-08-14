using System;

namespace MyRpg.Core.Models
{
    [Serializable]
    public class Cooldown
    {
        public string spellId;
        public float timeSinceCasted;
        
        public Spell Spell => LookupComponent.GetSpell(spellId); 
        public float RemainingCooldown => Spell.Cooldown - timeSinceCasted;
    }
}