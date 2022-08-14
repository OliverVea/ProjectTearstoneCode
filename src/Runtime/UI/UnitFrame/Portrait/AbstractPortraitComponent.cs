using MyRpg.Core.Components;
using UnityEngine;
using UnityEngine.UI;

namespace MyRpg.UI.UnitFrame
{
    public abstract class AbstractPortraitComponent : UiElement
    {
        
        [SerializeField] private Image portraitImage;
        [SerializeField] private Sprite fallbackImage;
        
        public void SetTarget(GameObject target)
        {
            portraitImage.sprite = fallbackImage;
            if (target == null) return;
            
            var statusComponent = target.GetComponent<IStatusComponent>();
            if (statusComponent == null) return;
            
            var sprite = statusComponent.GetPortrait();
            if (sprite == null) return;

            portraitImage.sprite = sprite;
            portraitImage.enabled = true;
        }
    }
}