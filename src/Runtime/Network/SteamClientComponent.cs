using System;
using System.Linq;
using Mirror;
using MyRpg.Core;
using MyRpg.Core.Events;
using MyRpg.Core.Models;
using MyRpg.Core.Network;
using Steamworks;
using UnityEngine;

namespace MyRpg.Network
{
    public class SteamClientComponent : MonoBehaviour, ISteamClientComponent
    {
        private MyNetworkManager _networkManager;
        
        private Callback<GameLobbyJoinRequested_t> _lobbyJoinRequest;
        private Callback<LobbyEnter_t> _lobbyEntered;
        private Callback<LobbyMatchList_t> _lobbyMatchList;

        [SerializeField] private LobbyInformation[] availableLobbies = Array.Empty<LobbyInformation>();
        [SerializeField] private CSteamID lobbySteamId;
        [SerializeField] private string hostAddress;

        private void Start()
        {
            _networkManager = GetComponent<MyNetworkManager>();
            
            _lobbyJoinRequest = Callback<GameLobbyJoinRequested_t>.Create(OnLobbyJoinRequested);
            _lobbyEntered = Callback<LobbyEnter_t>.Create(OnLobbyEntered);
            _lobbyMatchList = Callback<LobbyMatchList_t>.Create(OnLobbyMatchList);

            TaskManager.AddTask(RequestLobbyList, NetworkConstants.SecondsBetweenLobbyListRefresh);
        }

        private void RequestLobbyList()
        {
            SteamMatchmaking.AddRequestLobbyListStringFilter(NetworkConstants.GameKey, NetworkConstants.GameValue, ELobbyComparison.k_ELobbyComparisonEqual);
            SteamMatchmaking.RequestLobbyList();
        }

        private void OnLobbyMatchList(LobbyMatchList_t param)
        {
            availableLobbies = Enumerable.Range(0, (int)param.m_nLobbiesMatching)
                .Select(SteamMatchmaking.GetLobbyByIndex)
                .Select(GetLobbyInformation)
                .ToArray();

            MainMenuHandler.InvokeLobbyList(availableLobbies);
        }

        private LobbyInformation GetLobbyInformation(CSteamID steamId)
        {
            return new LobbyInformation
            {
                steamId = steamId,
                lobbyName = SteamMatchmaking.GetLobbyData(steamId, NetworkConstants.LobbyNameKey)
            };
        }

        private void OnLobbyJoinRequested(GameLobbyJoinRequested_t request)
        {
            SteamMatchmaking.JoinLobby(request.m_steamIDLobby);
        }

        private void OnLobbyEntered(LobbyEnter_t param)
        {
            if (NetworkServer.active) return;

            lobbySteamId = new CSteamID(param.m_ulSteamIDLobby);
            hostAddress = SteamMatchmaking.GetLobbyData(lobbySteamId, NetworkConstants.HostAddressKey);

            _networkManager.JoinGame(hostAddress);
        }

        public void JoinGame(CSteamID lobbySteamId)
        {
            SteamMatchmaking.JoinLobby(lobbySteamId);
        }
    }
}