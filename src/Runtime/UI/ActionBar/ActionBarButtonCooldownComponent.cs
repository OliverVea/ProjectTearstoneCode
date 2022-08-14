using System.Globalization;
using MyRpg.Core.Components;
using UnityEngine;
using UnityEngine.UI;

namespace MyRpg.UI.ActionBar
{
    public class ActionBarButtonCooldownComponent : MonoBehaviour
    {
        [SerializeField] private RectTransform cooldownCoverTransform;
        [SerializeField] private Image cooldownCoverImage;
        [SerializeField] private TMPro.TextMeshProUGUI cooldownText;
        
        private ICooldownComponent _cooldownComponent;
        
        private string _spellId;
        private float _buttonSize;

        private bool Initialized => _cooldownComponent != null && _spellId != null;

        public void Initialize(ICooldownComponent cooldownComponent, string spellId, float buttonSize)
        {
            _cooldownComponent = cooldownComponent;
            _spellId = spellId;
            _buttonSize = buttonSize;
            
            cooldownCoverTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, buttonSize - 6);
        }

        private void Update()
        {
            if (!Initialized) return;

            var cooldown = _cooldownComponent.GetCooldown(_spellId);

            cooldownCoverImage.enabled = cooldown != null;
            cooldownText.enabled = cooldown != null;
            if (cooldown == null) return;

            var ratio = cooldown.RemainingCooldown / cooldown.Spell.Cooldown;
            cooldownCoverTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, ratio * (_buttonSize - 6));

            var text = Mathf.Ceil(cooldown.RemainingCooldown).ToString(CultureInfo.InvariantCulture);
            cooldownText.SetText(text);
        }
    }
}