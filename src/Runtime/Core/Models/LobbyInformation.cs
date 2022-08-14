using System;
using Steamworks;

namespace MyRpg.Core.Models
{
    [Serializable]
    public class LobbyInformation
    {
        public CSteamID steamId;
        public string lobbyName;
    }
}