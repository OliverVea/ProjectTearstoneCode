using System;
using System.Linq;
using MyRpg.Characters;
using MyRpg.Core.Models;
using TMPro;
using UnityEngine;

namespace MyRpg.UI.CharacterSelection
{
    public class CharacterCreatorComponent : MonoBehaviour
    {
        private ClassArrowSelectorComponent _classArrowSelectorComponent;
        [SerializeField] private CharacterCardListComponent _characterCardListComponent;
        [SerializeField] private TMP_Text _characterNameField;

        private void Awake()
        {
            _classArrowSelectorComponent = GetComponentInChildren<ClassArrowSelectorComponent>();
        }

        private static KeyBinding[] GetSpellBindings(Class characterClass)
        {
            return characterClass switch
            {
                Class.Mage => new[]
                {
                    new KeyBinding { key = KeyCode.Alpha1, spellId = "fireball" },
                    new KeyBinding { key = KeyCode.Alpha2, spellId = "frostbolt" },
                    new KeyBinding { key = KeyCode.Alpha3, spellId = "frostnova" },
                },
                Class.Warrior => new[]
                {
                    new KeyBinding { key = KeyCode.Alpha1, spellId = "charge" },
                    new KeyBinding { key = KeyCode.Alpha2, spellId = "shieldbash" },
                    new KeyBinding { key = KeyCode.Alpha3, spellId = "taunt" },
                },
                Class.Priest => new[]
                {
                    new KeyBinding { key = KeyCode.Alpha1, spellId = "lesserheal" },
                    new KeyBinding { key = KeyCode.Alpha2, spellId = "renew" },
                    new KeyBinding { key = KeyCode.Alpha3, spellId = "powerwordshield" },
                },
                _ => Array.Empty<KeyBinding>()
            };
        }

        public void CreateCharacter()
        {
            var characterClass = _classArrowSelectorComponent.GetSelectedClass();
            if (characterClass == Class.None) return;
            
            var characterName = _characterNameField.text;
            if (InvalidCharacterName(characterName)) return;
            var spellBindings = GetSpellBindings(characterClass);

            var character = new Character
            {
                characterClass = characterClass,
                characterId = Guid.NewGuid().ToString(),
                characterName = characterName,
                spellBindings = spellBindings
            };

            CharacterLoader.SaveCharacter(character);
            _characterCardListComponent.Initialize();
            gameObject.SetActive(false);
        }

        private static bool InvalidCharacterName(string characterName)
        {
            if (characterName.Length < 1 || characterName.Length > 20) return false;

            const string acceptedCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
            if (characterName.Any(x => !acceptedCharacters.Contains(x))) return false;

            return true;
        }
    }
}