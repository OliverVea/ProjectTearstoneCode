using System;

namespace MyRpg.Core.Models
{
    [Serializable]
    public struct Character
    {
        public string characterId;
        public string characterName;
        public Class characterClass;
        
        // Level
        // Equipped items
        // Inventory
        // Talent Picks
        
        // Spells
        public KeyBinding[] spellBindings;
        
        public bool IsValid => characterName != "" && 
                               characterClass != Class.None && 
                               characterId != "" && 
                               characterId != Guid.Empty.ToString();
    }
}