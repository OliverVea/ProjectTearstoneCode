using System;
using System.Collections.Generic;
using System.Linq;
using MyRpg.Core.Events;
using MyRpg.Core.Models;
using MyRpg.Core.Network;
using MyRpg.UI.Menu;
using TMPro;
using UnityEngine;

namespace MyRpg.UI.Lobby
{
    public class LobbyListComponent : MonoBehaviour
    {
        [SerializeField] private GameObject noneObject;
        [SerializeField] private GameObject lobbyPrefab;
        [SerializeField] private List<GameObject> lobbies = new List<GameObject>();
        [SerializeField] private JoinLobbyComponent _joinLobbyComponent;

        private ISteamClientComponent _steamClientComponent;

        private void Awake()
        {
            _steamClientComponent = FindObjectsOfType<GameObject>()
                .Select(x => x.GetComponent<ISteamClientComponent>())
                .Single(x => x != null);
        }

        private void OnEnable()
        {
            MainMenuHandler.RegisterOnLobbyList(OnLobbyList);
        }
        private void OnDisable()
        {
            MainMenuHandler.UnregisterOnLobbyList(OnLobbyList);
        }

        private void Start()
        {
            UpdateLobbies(Array.Empty<LobbyInformation>());
        }

        private void OnLobbyList(LobbyInformation[] lobbies)
        {
            UpdateLobbies(lobbies);
        }

        private void UpdateLobbies(LobbyInformation[] lobbyInformations)
        {
            UpdateNoLobbies();
            if (!lobbyInformations.Any())
            {
                noneObject.GetComponentInChildren<TMP_Text>().SetText("No lobbies found. Still searching...");
                return;
            }

            noneObject.SetActive(false);

            for (var i = 0; i < lobbyInformations.Length; i++)
            {
                var lobbyInformation = lobbyInformations[i];
                
                var lobbyObject = Instantiate(lobbyPrefab, transform);
                lobbies.Add(lobbyObject);

                var lobbyEntryComponent = lobbyObject.GetComponent<LobbyEntryComponent>();
                lobbyEntryComponent.Initialize(i, lobbyInformation, ClickLobbyList);
            }
        }

        private void ClickLobbyList(int selectedIndex, LobbyInformation lobbyInformation)
        {
            for (var i = 0; i < lobbies.Count; i++) 
                lobbies[i].GetComponentInChildren<TabTextComponent>().SetSelectedState(i == selectedIndex);
            
            _joinLobbyComponent.SetLobbySteamId(lobbyInformation.steamId);
        }

        private void UpdateNoLobbies()
        {
            noneObject.SetActive(true);
            foreach (var lobby in lobbies) Destroy(lobby);
            lobbies = new List<GameObject>();
        }
    }
}