using MyRpg.Core.Events;
using MyRpg.Core.Models;
using MyRpg.Core.Network;
using Steamworks;
using UnityEngine;

namespace MyRpg.Network
{
    [DisallowMultipleComponent]
    public class PlayerNetworkComponent : MonoBehaviour
    {
        private ISelectedCharacterComponent _characterComponent;
        private ISteamClientComponent _steamClientComponent;
        private ISteamLobbyComponent _steamLobbyComponent;

        private void Awake()
        {
            _characterComponent = GetComponent<ISelectedCharacterComponent>();
            _steamClientComponent = GetComponent<ISteamClientComponent>();
            _steamLobbyComponent = GetComponent<ISteamLobbyComponent>();
        }

        private void OnEnable()
        {
            MainMenuHandler.RegisterOnCharacterSelected(OnCharacterSelected);
            MainMenuHandler.RegisterOnStartJoinGame(JoinGame);
            MainMenuHandler.RegisterOnStartHostGame(HostGame);
        }

        private void OnDisable()
        {
            MainMenuHandler.UnregisterOnCharacterSelected(OnCharacterSelected);
            MainMenuHandler.UnregisterOnStartJoinGame(JoinGame);
            MainMenuHandler.UnregisterOnStartHostGame(HostGame);
        }

        private void OnCharacterSelected(GameObject source, Character newCharacter)
        {
            Debug.Log($"{newCharacter.characterName} was selected.");
            _characterComponent.SelectCharacter(newCharacter);
        }

        public void JoinGame(CSteamID lobbySteamId)
        {
            _steamClientComponent.JoinGame(lobbySteamId);
        }

        public void HostGame()
        {
            _steamLobbyComponent.CreateLobby();
        }
    }
}