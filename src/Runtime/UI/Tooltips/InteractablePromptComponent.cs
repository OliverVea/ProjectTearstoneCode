using MyRpg.Core.Events;
using MyRpg.Core.Helpers;
using UnityEngine;

namespace MyRpg.UI.Tooltips
{
    public class InteractablePromptComponent : UiElement
    {
        private void Start()
        {
            gameObject.SetActive(false);
        }

        protected override void OnLocalPlayerAdded(GameObject localPlayer)
        {
            InteractableEventHandler.RegisterOnInteractablesChanged(OnInteractablesChanged);
        }

        protected override void OnLocalPlayerRemoved(GameObject localPlayer)
        {
            InteractableEventHandler.UnregisterOnInteractablesChanged(OnInteractablesChanged);
        }

        private void OnInteractablesChanged(GameObject player, int interactablesCount)
        {
            if (!LocalPlayerHelper.IsLocalPlayer(player)) return;
            gameObject.SetActive(interactablesCount > 0);
        }
    }
}