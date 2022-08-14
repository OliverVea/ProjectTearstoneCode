using MyRpg.Core.Attributes;
using MyRpg.Core.Components;
using MyRpg.Core.Events;
using MyRpg.Core.Models;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MyRpg.UI.Menu
{
    public class CharacterCardComponent : MonoBehaviour, ICharacterCardComponent
    {
        [SerializeField] private Image portraitImage;
        [SerializeField] private TMP_Text nameText;
        
        [ReadOnly] private Character _character;

        // UI Event
        public void OnClick()
        {
            Debug.Log($"Character card with character {_character.characterName} was clicked.");
            MainMenuHandler.InvokeCharacterSelected(gameObject, _character);
        }

        public void Initialize(Character character)
        {
            _character = character;

            var characterPrefab = LookupComponent.GetCharacterPrefab(character.characterClass);
            portraitImage.sprite = characterPrefab.GetComponent<IStatusComponent>().GetPortrait();

            nameText.text = character.characterName;
        }
    }
}