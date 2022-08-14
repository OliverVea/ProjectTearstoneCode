using System;
using System.Collections.Generic;
using System.Linq;
using MyRpg.Characters;
using MyRpg.Core.Attributes;
using MyRpg.Core.Components;
using MyRpg.Core.Events;
using MyRpg.Core.Models;
using UnityEngine;

namespace MyRpg.UI.CharacterSelection
{
    public class CharacterCardListComponent : MonoBehaviour
    {
        [SerializeField] private GameObject newCharacterCard;
        [SerializeField] private GameObject characterSelectionWindow;
        [SerializeField] private GameObject characterCardPrefab;
        [ReadOnly] private List<GameObject> characterCards = new List<GameObject>();

        private void OnEnable()
        {
            MainMenuHandler.RegisterOnCharacterSelected(OnCharacterSelected);
        }

        private void OnDisable()
        {
            MainMenuHandler.UnregisterOnCharacterSelected(OnCharacterSelected);
        }

        private void OnCharacterSelected(GameObject _, Character __)
        {
            characterSelectionWindow?.SetActive(false);
        }

        public void Initialize()
        {
            var characters = CharacterLoader.LoadCharacters();
            ClearCharacterCards();
            AddCharacterCards(characters);
        }

        private void AddCharacterCards(IEnumerable<Character> characters)
        {
            var newCharacterCards = characters.Select(CreateCharacterCard);
            characterCards.AddRange(newCharacterCards);
            newCharacterCard.transform.SetAsLastSibling();
        }

        private GameObject CreateCharacterCard(Character character)
        {
            var characterCardInstance = Instantiate(characterCardPrefab, transform);
            var characterCardComponent = characterCardInstance.GetComponent<ICharacterCardComponent>();
            characterCardComponent.Initialize(character);
            return characterCardInstance;
        }

        private void ClearCharacterCards()
        {
            foreach (var characterCard in characterCards) Destroy(characterCard);
            characterCards = new List<GameObject>();
        }
    }
}