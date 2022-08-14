using System.Linq;
using MyRpg.Core.Models;
using UnityEngine;

namespace MyRpg.Characters
{
    public class DefaultCharacterCreatorComponent : MonoBehaviour
    {
        [SerializeField] private bool _createDefaultCharacter;

        private void Start()
        {
            if (!_createDefaultCharacter) return;

            var characters = CharacterLoader.LoadCharacters();
            if (characters.Any(x => x.characterId == CharacterConstants.DefaultCharacterId)) return;
            
            var character = GetNewCharacter();
            CharacterLoader.SaveCharacter(character);
        }

        private static Character GetNewCharacter()
        {
            return new Character
            {
                characterClass = Class.Mage,
                characterId = CharacterConstants.DefaultCharacterId,
                characterName = "Character Name",
                spellBindings = new[]
                {
                    new KeyBinding
                    {
                        key = KeyCode.Alpha1,
                        spellId = "fireball"
                    }
                },
             };
        }
    }
}