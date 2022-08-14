using System.Linq;
using MyRpg.Characters;
using MyRpg.Core.Constants;
using MyRpg.Core.Events;
using MyRpg.Core.Models;
using TMPro;
using UnityEngine;

namespace MyRpg.UI.Menu
{
    public class CharacterNameComponent : MonoBehaviour
    {
        private TMP_Text _text;

        private void Awake()
        {
            _text = GetComponent<TMP_Text>();
            var selectedCharacterId = PlayerPrefs.GetString(ConstantValues.PrefKeys.SelectedCharacter, "");
            var selectedCharacter = CharacterLoader.LoadCharacters()
                .FirstOrDefault(x => x.characterId == selectedCharacterId);
            var text = selectedCharacter.IsValid ? selectedCharacter.characterName : "";
            _text.SetText(text);
        }

        private void OnEnable()
        {
            MainMenuHandler.RegisterOnCharacterSelected(OnCharacterSelected);
        }

        private void OnCharacterSelected(GameObject source, Character character)
        {
            _text.SetText(character.characterName);
        }
    }
}