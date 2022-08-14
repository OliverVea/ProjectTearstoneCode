using MyRpg.Core;
using MyRpg.Core.Events;
using MyRpg.Core.Network;
using Steamworks;
using UnityEngine;

namespace MyRpg.Network
{
    public class SteamLobbyComponent : MonoBehaviour, ISteamLobbyComponent
    {
        private IMyNetworkManager _networkManager;
        private ISelectedCharacterComponent _characterComponent;

        private Callback<LobbyCreated_t> _lobbyCreated;
        [SerializeField] private CSteamID lobbySteamId;
        [SerializeField] private string hostAddress;
        private bool _hasLobby;

        [field: SerializeField] public ELobbyType LobbyType { get; private set; }
        [field: SerializeField] public int MaxPlayerCount { get; private set; }

        private void Awake()
        {
            _networkManager = GetComponent<IMyNetworkManager>();
            _characterComponent = GetComponent<ISelectedCharacterComponent>();
        }

        private void OnEnable()
        {
            _lobbyCreated = Callback<LobbyCreated_t>.Create(OnLobbyCreated);
            
            MainMenuHandler.RegisterOnSetLobbyType(SetLobbyType);
        }

        private void OnDisable()
        {
            MainMenuHandler.UnregisterOnSetLobbyType(SetLobbyType);
        }

        private void Start()
        {
            TaskManager.AddTask(LobbyHeartbeat, NetworkConstants.SecondsBetweenLobbyHeartbeat);
        }

        public void CreateLobby()
        {
            if (!_characterComponent.IsCharacterSelected())
            {
                Debug.LogError("A character must be selected before creating a lobby.");
                return;
            }

            Debug.Log("Creating lobby...");
            SteamMatchmaking.CreateLobby(LobbyType, MaxPlayerCount);
        }

        public void SetLobbyType(string lobbyType)
        {
            LobbyType = lobbyType switch
            {
                "public" => ELobbyType.k_ELobbyTypePublic,
                "friends" => ELobbyType.k_ELobbyTypeFriendsOnly,
                "private" => ELobbyType.k_ELobbyTypePrivate,
                _ => ELobbyType.k_ELobbyTypePrivate
            };
        }
        
        private void OnLobbyCreated(LobbyCreated_t lobby)
        {
            if (lobby.m_eResult != EResult.k_EResultOK)
            {
                Debug.LogError($"Steam lobby creation failed with error {lobby.m_eResult}.");
                return;
            }

            _hasLobby = true;
            
            Debug.Log("Lobby successfully created. Starting server...");

            lobbySteamId = new CSteamID(lobby.m_ulSteamIDLobby);
            hostAddress = SteamUser.GetSteamID().ToString();
            SteamMatchmaking.SetLobbyData(lobbySteamId, NetworkConstants.HostAddressKey, hostAddress);
            SteamMatchmaking.SetLobbyData(lobbySteamId, NetworkConstants.GameKey, NetworkConstants.GameValue);
            SteamMatchmaking.SetLobbyData(lobbySteamId, NetworkConstants.LobbyNameKey, GetLobbyName());

            var success = _networkManager.StartGame();
            
            if (success) Debug.Log("Server started successfully.");
            else Debug.LogError("Failed starting server.");
        }

        private static string GetLobbyName()
        {
            var steamPlayerName = SteamFriends.GetPersonaName();
            return $"{steamPlayerName}'s lobby";
        }

        private void LobbyHeartbeat()
        {
            var gameValue = SteamMatchmaking.GetLobbyData(lobbySteamId, NetworkConstants.GameKey);
            var alive = gameValue == NetworkConstants.GameValue;

            var statusText = alive ? "Up" : "Down";
            if (_hasLobby != alive) Debug.LogError($"Lobby was unexpectedly {statusText}.");
            else Debug.Log($"Lobby was expectedly {statusText}.");
        }
    }
}