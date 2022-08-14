using Mirror;
using MyRpg.Core;
using MyRpg.Core.Components;
using TMPro;
using UnityEngine;

namespace MyRpg.UI.UnitFrame
{
    public class PlayerNameComponent : UiElement
    {
        [SerializeField] private TMP_Text nameText;

        protected override void OnLocalPlayerAdded(GameObject player)
        {
            var statusComponent = player.GetComponent<IStatusComponent>();
            if (statusComponent != null) nameText.text = statusComponent.GetName();
        }
    }
}