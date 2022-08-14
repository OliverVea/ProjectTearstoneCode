using Steamworks;

namespace MyRpg.Core.Network
{
    public interface ISteamClientComponent
    {
        void JoinGame(CSteamID lobbySteamId);
    }
}