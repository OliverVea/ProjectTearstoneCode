using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace MyRpg.UI.Tooltips
{
    public abstract class AbstractTooltipTriggerComponent : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        protected abstract string GetTitle();
        protected abstract (string, string)[] GetAttributes();
        protected abstract string GetText();


        public void OnPointerEnter(PointerEventData eventData)
        {
            var title = GetTitle();
            var attributes = GetAttributes();
            var text = GetText();
            
            TooltipManager.Show(title, attributes, text);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            TooltipManager.Hide();
        }
    }
}