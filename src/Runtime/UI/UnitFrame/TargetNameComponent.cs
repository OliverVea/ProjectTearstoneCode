using MyRpg.Core.Components;
using TMPro;
using UnityEngine;

namespace MyRpg.UI.UnitFrame
{
    public class TargetNameComponent : TargetElement
    {
        
        [SerializeField] private TMP_Text nameText;

        protected override void OnTargetChanged(GameObject oldTarget, GameObject newTarget)
        {
            nameText.text = "";

            if (newTarget == null) return;
            
            var statusComponent = newTarget.GetComponent<IStatusComponent>();
            if (statusComponent == null) return;
            
            nameText.text = statusComponent.GetName();
        }
    }
}