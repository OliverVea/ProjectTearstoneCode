using Mirror;
using UnityEngine;

namespace MyRpg.Core.Helpers
{
    public class LocalPlayerHelper
    {
        public static bool IsLocalPlayer(GameObject gameObject)
            => NetworkClient.localPlayer.gameObject != null && gameObject == NetworkClient.localPlayer.gameObject;
    }
}