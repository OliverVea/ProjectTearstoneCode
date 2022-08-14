using System.Collections.Generic;
using System.Linq;
using MyRpg.Core;
using MyRpg.Core.Components;
using MyRpg.Core.Models;
using MyRpg.UI.Tooltips;
using UnityEngine;

namespace MyRpg.UI.ActionBar
{
    [ExecuteInEditMode]
    public class ActionBarComponent : UiElement, IActionBarComponent
    {
        [SerializeField] private float buttonSize;
        [SerializeField] private float buttonSpacing;
        [SerializeField] private GameObject button;

        private ICooldownComponent _cooldownComponent;
        private IPlayerController _playerController;

        private KeyBinding[] _keyBindings;

        protected override void OnLocalPlayerAdded(GameObject player) 
        {
            _cooldownComponent = player.GetComponent<ICooldownComponent>();
            _playerController = player.GetComponent<IPlayerController>();
            _keyBindings = _playerController.GetSpellBindings();
            if (_keyBindings != null) UpdateButtons();
        }

        private void UpdateButtons()
        {
            DeleteAllButtons();
            CreateNewButtons();
        }

        private void DeleteAllButtons()
        {
            var oldButtons = GetComponentsInChildren<RectTransform>()
                .Where(x => x.CompareTag("ActionBarButton"))
                .ToArray();
            
            for (int i = 0; i < oldButtons.Length; i++)
                DestroyImmediate(oldButtons[i].gameObject);
        }

        private void CreateNewButtons()
        {
            var totalLength = (_keyBindings.Length - 1) * buttonSize + (_keyBindings.Length - 1) * buttonSpacing;
            for (int i = 0; i < _keyBindings.Length; i++) CreateButton(_keyBindings[i], i, totalLength);
        }

        private void CreateButton(KeyBinding keyBinding, int buttonIndex, float totalLength)
        {
                var xPosition = buttonIndex * buttonSize + buttonIndex * buttonSpacing - totalLength / 2;
                
                var buttonInstance = Instantiate(button, transform);
                buttonInstance.GetComponent<ActionBarButtonLayoutComponent>().Initialize(keyBinding, new Vector2(xPosition, 0), buttonSize);
                buttonInstance.GetComponent<ActionBarButtonCooldownComponent>().Initialize(_cooldownComponent, keyBinding.spellId, buttonSize);

                var spell = LookupComponent.GetSpell(keyBinding.spellId);
                buttonInstance.GetComponent<SpellTooltipTriggerComponent>()?.SetSpell(spell);
        }
    }
}
