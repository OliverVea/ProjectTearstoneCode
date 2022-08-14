using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MyRpg.UI.Tooltips
{
    [ExecuteInEditMode]
    public class Tooltip : MonoBehaviour
    {
        [SerializeField] private TMP_Text header;
        [SerializeField] private TMP_Text content;
        [SerializeField] private LayoutElement layoutElement;
        [SerializeField] private int characterWrapLimit;

        private void Update()
        {
            var rectTransform = transform as RectTransform;
            var pivotX = Input.mousePosition.x > Screen.width / 2 ? 1 : 0;
            var pivotY = Input.mousePosition.y > Screen.height / 2 ? 1 : 0;
            
            rectTransform.pivot = new Vector2(pivotX, pivotY);
            
            var wrap = header.text.Length > characterWrapLimit || content.text.Length > characterWrapLimit;
            layoutElement.enabled = wrap;
            transform.position = Input.mousePosition + new Vector3(40, 0, 0) * (pivotX * -2 + 1);
        }

        public void SetText(string title, (string, string)[] attributes, string text)
        {
            header.SetText(title);

            var contentElements = attributes.Select(x => $"{x.Item1}: {x.Item2}").Concat(new [] { text });
            var contentText = string.Join("\n", contentElements);
            content.SetText(contentText);
        }
    }
}