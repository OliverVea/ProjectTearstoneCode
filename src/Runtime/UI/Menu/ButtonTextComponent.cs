using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MyRpg.UI.Menu
{
    public class ButtonTextComponent : MonoBehaviour
    {
        [SerializeField] private Vector2 displacement = Vector2.zero;
        [SerializeField] private Color activeColor;
        [SerializeField] private Color disabledColor;
        [field: SerializeField] public bool Disabled { get; private set; }
        [field: SerializeField] public bool DisablesButton { get; private set; }

        private RectTransform _rectTransform;
        private TMP_Text _text;
        private Button _parentButton;
        
        Vector3 _defaultPosition;
     
        void Start()
        {
            _rectTransform = GetComponent<RectTransform>();
            _text = GetComponent<TMP_Text>();
            _parentButton = GetComponentInParent<Button>();
            _defaultPosition = _rectTransform.localPosition;
            _text.color = Disabled ? disabledColor : activeColor;
        }
 
        public void Down()
        {
            if (Disabled) return;
            
            _rectTransform.localPosition = _defaultPosition + (Vector3)displacement;
        }
 
        public void Up()
        {
            _rectTransform.localPosition = _defaultPosition;
        }

        public void SetDisabledState(bool newState)
        {
            Disabled = newState;
            _parentButton.interactable = !Disabled;
            _text.color = Disabled ? disabledColor : activeColor;
            if (Disabled) _rectTransform.localPosition = _defaultPosition;
        }
    }
}
