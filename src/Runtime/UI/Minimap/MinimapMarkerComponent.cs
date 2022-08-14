using MyRpg.Core.Events;
using UnityEngine;

namespace MyRpg.UI.Minimap
{
    public class MinimapMarkerComponent : MonoBehaviour
    {
        private IDeathEventHandler _deathEventHandler;

        private void Awake()
        {
            _deathEventHandler = transform.parent.GetComponent<IDeathEventHandler>();
        }

        private void OnEnable()
        {
            if (_deathEventHandler == null) return;
            _deathEventHandler.RegisterOnDeath(OnDeath);
        }

        private void OnDisable()
        {
            if (_deathEventHandler == null) return;
            _deathEventHandler.UnregisterOnDeath(OnDeath);
        }

        private void OnDeath()
        {
            LeanTween.alpha(gameObject, 0, 0.2f);
            LeanTween.scale(gameObject, Vector3.zero, 0.2f)
                .setOnComplete(() => gameObject.SetActive(false));
        }
    }
}