using MyRpg.Core.Constants;
using MyRpg.Core.Models;
using UnityEngine;
using UnityEngine.UI;

namespace MyRpg.UI.ActionBar
{
    public class ActionBarButtonLayoutComponent : MonoBehaviour
    {
        [SerializeField] private Image iconImage;
        [SerializeField] private RectTransform iconTransform;
        [SerializeField] private TMPro.TextMeshProUGUI keyBindText;
        
        public void Initialize(KeyBinding keyBinding, Vector2 position, float buttonSize)
        {
            SetButtonTransform(position, buttonSize);
            SetIconTransform(buttonSize);
            SetIconImage(keyBinding);
            SetKeyBindText(keyBinding);
        }

        private void SetButtonTransform(Vector2 position, float buttonSize)
        {
            transform.localPosition = position;
            
            ((RectTransform)transform).SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, buttonSize);
            ((RectTransform)transform).SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, buttonSize);
        }

        private void SetIconTransform(float buttonSize)
        {
            iconTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, buttonSize - 6);
            iconTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, buttonSize - 6);
        }

        private void SetIconImage(KeyBinding keyBinding)
        {
            var spell = LookupComponent.GetSpell(keyBinding.spellId);
            iconImage.sprite = spell?.Icon;
            iconImage.enabled = true;
        }

        private void SetKeyBindText(KeyBinding keyBinding)
        {
            var keyName = KeyCodeNames.GetKeyName(keyBinding.key);
            keyBindText.SetText(keyName);
        }
    }
}
