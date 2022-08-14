using System.Linq;
using Mirror;
using MyRpg.Core.Modifiers;
using UnityEngine;
using UnityEngine.UI;

namespace MyRpg.UI.UnitFrame
{
    public class ShieldTracker : UiElement
    {
        [SerializeField] private Graphic _indicator;

        private IAbsorptionProvider[] _absorptionProviders;
        
        
        protected override void OnLocalPlayerAdded(GameObject player)
        {
            _absorptionProviders = player.GetComponents<IAbsorptionProvider>();
        }

        private void Update()
        {
            var hasShield = _absorptionProviders?.Any(x => x.HasAbsorption()) ?? false;
            Show(hasShield);
        }

        private void Show(bool state)
            => _indicator.enabled = state;
    }
}