using System;
using UnityEngine;

namespace MyRpg.Characters
{
    [Serializable]
    public class CharacterEntity
    {
        public string characterId = Guid.Empty.ToString();
        public string characterName;
        public string characterClass;
        
        // Level
        public int characterLevel = 1;
        public float characterExperience;

        public string[] spellKeys = Array.Empty<string>();
        public string[] spellIds = Array.Empty<string>();

        [field: SerializeField] public int Version { get; private set; } = 1;
    }
}