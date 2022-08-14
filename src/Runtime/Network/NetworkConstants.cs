using Steamworks;

namespace MyRpg.Network
{
    public static class NetworkConstants
    {
        public const string GameValue = "ProjectTearstone";
        
        public const float SecondsBetweenLobbyListRefresh = 5f;
        public const float SecondsBetweenLobbyHeartbeat = 30f;
        
        // Lobby data keys
        public const string HostAddressKey = "HostAddressKey";
        public const string GameKey = "GameKey";
        public const string LobbyNameKey = "LobbyNameKey";
    }
}