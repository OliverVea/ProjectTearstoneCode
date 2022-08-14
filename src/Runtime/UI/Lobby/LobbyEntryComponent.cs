using System;
using MyRpg.Core.Models;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MyRpg.UI.Lobby
{
    public class LobbyEntryComponent : MonoBehaviour
    {
        [SerializeField] private TMP_Text text;
        private Button _button;
        private Action<int, LobbyInformation> _onClick;
        private int _index;
        private LobbyInformation _lobbyInformation;

        private void Awake()
        {
            _button = GetComponent<Button>();
        }

        private void OnDisable()
        {
            _button.onClick.RemoveListener(OnButtonClick);
        }

        private void OnButtonClick()
        {
            _onClick?.Invoke(_index, _lobbyInformation);
        }

        public void Initialize(int index, LobbyInformation lobbyInformation, Action<int, LobbyInformation> onClick)
        {
            _index = index;
            _lobbyInformation = lobbyInformation;
            
            text.text = lobbyInformation.lobbyName;
            _onClick += onClick;
            
            _button.onClick.AddListener(OnButtonClick);
        }
    }
}