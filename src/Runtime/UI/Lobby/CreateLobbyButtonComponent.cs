using MyRpg.Core.Constants;
using MyRpg.UI.Menu;
using UnityEngine;
using UnityEngine.UI;

namespace MyRpg.UI.Lobby
{
    public class CreateLobbyButtonComponent : MonoBehaviour
    {
        private Button _button;
        private ButtonTextComponent _buttonTextComponent;

        protected void Awake()
        {
            _button = GetComponent<Button>();
            _buttonTextComponent = GetComponentInChildren<ButtonTextComponent>();
        }
        
        private void Update()
        {
            var selectedCharacter = PlayerPrefs.GetString(ConstantValues.PrefKeys.SelectedCharacter, "");
            var buttonEnabled = selectedCharacter != "";
            _button.interactable = buttonEnabled;
            _buttonTextComponent.SetDisabledState(!buttonEnabled);
        }
    }
}