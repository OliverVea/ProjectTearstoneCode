using System.Collections.Generic;
using UnityEngine;

namespace MyRpg.Core.Network
{
    public interface IMyNetworkManager
    {
        bool StartGame();
        bool JoinGame(string hostAddress);
        void ResetLevel();
    }
}