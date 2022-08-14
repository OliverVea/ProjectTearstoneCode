using TMPro;
using UnityEngine;

namespace MyRpg.UI.Menu
{
    public class TabTextComponent : MonoBehaviour
    {
        [SerializeField] private Color _selectedTextColor = Color.magenta;
        [SerializeField] private Color _defaultTextColor = Color.magenta;

        [SerializeField] private bool _selected;

        private TMP_Text _text;
     
        void Start()
        {
            _text = GetComponent<TMP_Text>();
            _text.color = _selected ? _selectedTextColor : _defaultTextColor;
        }

        public void SetSelectedState(bool newState)
        {
            _selected = newState;
            _text.color = _selected ? _selectedTextColor : _defaultTextColor;
        }
    }
}