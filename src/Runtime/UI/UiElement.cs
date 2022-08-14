using Mirror;
using MyRpg.Core;
using MyRpg.Core.Helpers;
using UnityEngine;

namespace MyRpg.UI
{
    public class UiElement : NetworkBehaviour, IPlayerJoinedSubscriber
    {

        protected virtual void OnLocalPlayerAdded(GameObject localPlayer)
        {
            
        }

        protected virtual void OnLocalPlayerRemoved(GameObject localPlayer)
        {
            
        }

        public virtual void OnPlayerCharacterAdded(GameObject player)
        {
            if (LocalPlayerHelper.IsLocalPlayer(player)) OnLocalPlayerAdded(player);
        }

        public virtual void OnPlayerCharacterRemoved(GameObject player)
        {
            if (LocalPlayerHelper.IsLocalPlayer(player)) OnLocalPlayerRemoved(player);
        }
    }
}