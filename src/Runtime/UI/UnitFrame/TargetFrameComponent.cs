using UnityEngine;
using UnityEngine.UI;

namespace MyRpg.UI.UnitFrame
{
    public class TargetFrameComponent : TargetElement
    {

        [SerializeField] private Graphic[] graphics;

        private void Start()
        {
            OnTargetChanged(null, null);
        }

        protected override void OnTargetChanged(GameObject oldTarget, GameObject newTarget)
            => Show(newTarget != null);

        private void Show(bool state)
        {
            foreach (var graphic in graphics) graphic.enabled = state;
        }
    }
}