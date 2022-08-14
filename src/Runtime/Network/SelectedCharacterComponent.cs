using System;
using System.Linq;
using MyRpg.Characters;
using MyRpg.Core.Constants;
using MyRpg.Core.Models;
using MyRpg.Core.Network;
using UnityEngine;

namespace MyRpg.Network
{
    public class SelectedCharacterComponent : MonoBehaviour, ISelectedCharacterComponent
    {
        [SerializeField] private Character character;

        public void Awake()
        {
            if (!PlayerPrefs.HasKey(ConstantValues.PrefKeys.SelectedCharacter)) return;
            
            var selectedCharacterId = PlayerPrefs.GetString(ConstantValues.PrefKeys.SelectedCharacter);
            if (string.IsNullOrWhiteSpace(selectedCharacterId)) return;
            
            var characters = CharacterLoader.LoadCharacters();
            character = characters.FirstOrDefault(x => x.characterId == selectedCharacterId);
        }

        public void SelectCharacter(Character newCharacter)
        {
            character = newCharacter;
            PlayerPrefs.SetString(ConstantValues.PrefKeys.SelectedCharacter, newCharacter.characterId);
        }

        public bool IsCharacterSelected()
        {
            return character.IsValid;
        }

        public void UnselectCharacter()
        {
            character.characterName = "";
            character.characterClass = Class.None;
        }

        public Character GetCharacter()
        {
            return character;
        }
    }
}