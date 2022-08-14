using System.Collections.Generic;
using System.Linq;
using Mirror;
using MyRpg.Core;
using MyRpg.Core.Helpers;
using UnityEngine;

namespace MyRpg.Components
{
    public class InteractableAreaComponent : NetworkBehaviour, IPlayerJoinedSubscriber
    {
        private Collider2D _collider;
        private Transform _playerTransform;
        private IPlayerController _playerController;

        private readonly BoolHelper _showBoolHelper = new BoolHelper();

        [SerializeField] private List<GameObject> interactableComponents = new List<GameObject>();
        [SerializeField] private float secondsBetweenUpdates = 0.1f;
        [SerializeField] private bool hostOnly;
        
        private float _lastUpdate = 100f;

        private void Awake()
        {
            _collider = GetRequiredComponent<Collider2D>();
        }

        [ClientCallback]
        private void Update()
        {
            if (hostOnly && !isServer) return;
            
            _lastUpdate += Time.deltaTime;
            if (_lastUpdate <= secondsBetweenUpdates) return;
            _lastUpdate = -secondsBetweenUpdates * Random.value;

            var hitColliders = new List<Collider2D>();
            _collider.OverlapCollider(new ContactFilter2D(), hitColliders);

            var show = hitColliders.Any(x => x.transform == _playerTransform);
            _showBoolHelper.Update(show);

            if (_showBoolHelper.NowTrue) EnableInteraction();
            if (_showBoolHelper.NowFalse) DisableInteraction();
        }

        private void EnableInteraction()
        {
            if (_playerController == null) return;

            _playerController.RegisterInteractables(interactableComponents);
        }

        private void DisableInteraction()
        {
            if (_playerController == null) return;
            
            _playerController.UnregisterInteractables(interactableComponents);
        }

        public void OnPlayerCharacterAdded(GameObject player)
        {
            if (!LocalPlayerHelper.IsLocalPlayer(player)) return;
            
            _playerTransform = player.transform;
            _playerController = player.GetComponent<IPlayerController>();
        }

        public void OnPlayerCharacterRemoved(GameObject player)
        {
            if (!LocalPlayerHelper.IsLocalPlayer(player)) return;
            
            _playerTransform = null;
            _playerController = null;
        }
    }
}