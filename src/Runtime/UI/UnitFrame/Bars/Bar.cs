using UnityEngine;
using UnityEngine.UI;

namespace MyRpg.UI.UnitFrame.Bars
{
    public abstract class Bar : UiElement
    {
        [SerializeField] private Graphic frame;
        [SerializeField] private Graphic fill;
        [SerializeField] private BarColor barColor;

        protected void UpdateValue(float currentValue, float maxValue)
        {
            float value;
            if (maxValue == 0) value = 0;
            else value = currentValue / maxValue;
            
            var color = GetColor(value);

            fill.rectTransform.localScale = new Vector3(value, 1);
            fill.color = color;
        }

        private Color GetColor(float value)
        {
            var i = GetColorIndex(value);
            return barColor.Colors[i];
        }

        private int GetColorIndex(float value)
        {
            for (int i = 0; i < barColor.Thresholds.Count; i++) 
                if (value < barColor.Thresholds[i]) 
                    return i;
            return barColor.Thresholds.Count;
        }

        protected void Show(bool state)
        {
            frame.enabled = state;
            fill.enabled = state;
        }

        protected abstract void SetTarget(GameObject target);
        protected abstract void ClearTarget();
    }
}
