using Mirror;
using MyRpg.Core.Components;
using UnityEngine;
using Random = UnityEngine.Random;

namespace MyRpg.Components.Abilities
{
    public class LightningStormComponent : NetworkBehaviour, IInteractableComponent
    {
        [SerializeField] private float duration;
        [SerializeField] private float radius;
        [SerializeField] private float secondsBetweenStrikes;
        [SerializeField] private GameObject strikePrefab;
        [SerializeField] private int spawnsAtATime;
        [SerializeField] private bool stormEnabled;

        private float _lastStrike = float.MaxValue;
        private float _age;

        [ServerCallback]
        private void Update()
        {
            if (!stormEnabled) return;
            
            _age += Time.deltaTime;
            if (_age > duration) ResetStorm(false);
            
            _lastStrike += Time.deltaTime;
            if (_lastStrike < secondsBetweenStrikes) return;

            for (int i = 0; i < spawnsAtATime; i++)
            {
                var strikePosition = (Vector3)Random.insideUnitCircle * radius;
                var strike = Instantiate(strikePrefab, transform.position + strikePosition, Quaternion.identity);
                NetworkServer.Spawn(strike);
            }
            
            _lastStrike = 0;
        }

        private void OnDrawGizmos()
        {
            if (!stormEnabled) return;
            
            Gizmos.color = new Color(1, 0, 0, 0.3f );
            Gizmos.DrawWireSphere(transform.position, radius);
        }

        private void ResetStorm(bool setStormEnabled)
        {
            _lastStrike = float.MaxValue;
            _age = 0;
            stormEnabled = setStormEnabled;
        }

        public void ServerInteract()
        {
            ResetStorm(true);
        }
    }
}