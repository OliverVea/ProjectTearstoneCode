using MyRpg.Core.Components;
using MyRpg.Core.Constants;
using MyRpg.Core.Events;
using MyRpg.UI.Menu;
using Steamworks;
using UnityEngine;
using UnityEngine.UI;

namespace MyRpg.UI.Lobby
{
    public class JoinLobbyComponent : MonoBehaviour, IJoinLobbyComponent
    {
        private CSteamID _currentlySelectedLobby = CSteamID.Nil;

        private Button _button;
        private ButtonTextComponent _buttonTextComponent;

        protected void Awake()
        {
            _button = GetComponent<Button>();
            _buttonTextComponent = GetComponentInChildren<ButtonTextComponent>();
        }

        private void OnEnable()
        {
            _button.onClick.AddListener(OnButtonClick);
        }

        private void OnButtonClick()
        {
            if (_currentlySelectedLobby == CSteamID.Nil) return;
            MainMenuHandler.InvokeStartJoinGame(_currentlySelectedLobby);
        }

        private void Start()
        {
            _button.interactable = false;
        }

        private void Update()
        {
            var selectedCharacter = PlayerPrefs.GetString(ConstantValues.PrefKeys.SelectedCharacter, "");
            var buttonEnabled = selectedCharacter != "" && _currentlySelectedLobby != CSteamID.Nil;
            _button.interactable = buttonEnabled;
            _buttonTextComponent.SetDisabledState(!buttonEnabled);
        }

        public void SetLobbySteamId(CSteamID lobbySteamId)
        {
            _currentlySelectedLobby = lobbySteamId;
        }
    }
}