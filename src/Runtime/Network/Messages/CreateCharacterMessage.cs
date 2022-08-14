using Mirror;
using MyRpg.Core.Models;

namespace MyRpg.Network.Messages
{
    public struct CreateCharacterMessage : NetworkMessage
    {
        public Character Character;
    }
}