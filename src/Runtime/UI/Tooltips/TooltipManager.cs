using UnityEngine;

namespace MyRpg.UI.Tooltips
{
    public class TooltipManager : MonoBehaviour
    {
        private static TooltipManager _current;

        public Tooltip tooltip;

        private void Awake()
        {
            _current = this;
        }

        public static void Show(string title, (string, string)[] attributes, string text)
        {
            _current.tooltip.gameObject.SetActive(true);
            _current.tooltip.SetText(title, attributes, text);
        }

        public static void Hide()
        {
            _current.tooltip.gameObject.SetActive(false);
        }
    }
}