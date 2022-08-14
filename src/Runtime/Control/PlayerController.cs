using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Mirror;
using MyRpg.Core;
using MyRpg.Core.Components;
using MyRpg.Core.Constants;
using MyRpg.Core.Events;
using MyRpg.Core.Models;
using MyRpg.Core.Network;
using UnityEngine;
using UnityEngine.AI;

namespace MyRpg.Runtime.Control
{
    public class PlayerController : NetworkBehaviour, IPlayerController
    {
        private IDirectionMovementComponent _movementComponent;
        private IAttackingComponent _attackingComponent;
        private ITargetingComponent _targetingComponent;
        private IStatusComponent _statusComponent;
        private ICastingComponent _castingComponent;
        private IMyNetworkManager _networkManager;
        
        private IActionBarComponent _actionBar;
        private Camera _camera;
        [CanBeNull] private NavMeshAgent _agent;

        [SerializeField] private List<GameObject> enemiesOnScreen;
        
        private readonly SyncList<KeyBinding> _spellBindings = new SyncList<KeyBinding>();

        private readonly SyncList<GameObject> _interactables = new SyncList<GameObject>();
        private readonly RaycastHit2D[] _rayCastHits = new RaycastHit2D[100];

        private void Awake()
        {
            _targetingComponent = GetRequiredComponent<ITargetingComponent>();
            _attackingComponent = GetRequiredComponent<IAttackingComponent>();
            _movementComponent = GetRequiredComponent<IDirectionMovementComponent>();            
            _castingComponent = GetRequiredComponent<ICastingComponent>();
            _statusComponent = GetRequiredComponent<IStatusComponent>();

            _agent = GetComponent<NavMeshAgent>();
            
            _actionBar = FindObjectsOfType<MonoBehaviour>()
                .OfType<IActionBarComponent>().FirstOrDefault();
         
            _networkManager = GameObject.FindWithTag("NetworkManager").GetComponent<IMyNetworkManager>();
            
            _camera = Camera.main;
        }

        private List<IPlayerJoinedSubscriber> _subscribers;

        public override void OnStartLocalPlayer()
        {
            var gameObjects = FindObjectsOfType<GameObject>();
            _subscribers = gameObjects.SelectMany(x => x.GetComponents<IPlayerJoinedSubscriber>()).ToList();
            foreach (var subscriber in _subscribers) subscriber.OnPlayerCharacterAdded(NetworkClient.localPlayer.gameObject);
        }

        private void Start()
        {
            if (_agent != null) _agent.enabled = false;
        }

        private void OnDisable()
        {
            if (_agent != null) _agent.enabled = true;
            foreach (var subscriber in _subscribers) 
                if (subscriber != null) 
                    subscriber.OnPlayerCharacterRemoved(NetworkClient.localPlayer.gameObject);
        }

        [ClientCallback]
        private void FixedUpdate()
        {
            if (_actionBar != null && !isLocalPlayer || !hasAuthority || _statusComponent.IsDead()) return;

            var direction = GetMovementDirection();

            _movementComponent.SetDirection(direction);

            UpdateEnemiesOnScreen();
        }

        private void HandleInteractables()
        {
            var use = Input.GetKeyDown(ConstantValues.UseKey);
            if (use) CommandUse();
        }

        [Command]
        private void CommandUse()
        {
            foreach (var interactable in _interactables)
                interactable.GetComponent<IInteractableComponent>()?.ServerInteract();
        }
        

        private void UpdateEnemiesOnScreen()
        {
            enemiesOnScreen = GameObject.FindGameObjectsWithTag("Character")
                .Where(x => IsHostile(x) && IsOnScreen(x) && IsAlive(x))
                .ToList();
        }

        private bool IsOnScreen(GameObject character)
        {
            Vector3 screenPoint = _camera.WorldToViewportPoint(character.transform.position);
            return screenPoint.x > 0 && screenPoint.x < 1 && screenPoint.y > 0 && screenPoint.y < 1;
        }

        private bool IsHostile(GameObject character)
        {
            var characterStatus = character.GetComponent<IStatusComponent>();
            return FactionRelations.IsAttackable(_statusComponent.GetFaction(), characterStatus.GetFaction());
        }

        private bool IsAlive(GameObject character)
        {
            var characterStatus = character.GetComponent<IStatusComponent>();
            return !characterStatus.IsDead();
        }

        private Vector2 GetMovementDirection()
        {
            var direction = Vector2.zero;

            if (Input.GetKey(KeyCode.W)) direction.y += 1;
            if (Input.GetKey(KeyCode.S)) direction.y -= 1;
            if (Input.GetKey(KeyCode.D)) direction.x += 1;
            if (Input.GetKey(KeyCode.A)) direction.x -= 1;

            return direction;
        }


        [ClientCallback]
        private void Update()
        {
            if (!isLocalPlayer || !hasAuthority) return;

            TargetHandling();
            EscapeHandling();
            HandleInteractables();
            PartySelectionHandling();
            ResetHandling();

            if (_statusComponent.IsDead()) return;

            foreach (var binding in _spellBindings)
            {
                if (Input.GetKeyDown(binding.key)) _castingComponent.CommandCast(binding.spellId, _targetingComponent.Target);
            }
        }

        private void ResetHandling()
        {
            if (!Input.GetKeyDown(ConstantValues.ResetKey) || 
                !isServer) return;

            _networkManager.ResetLevel();
        }

        private GameObject[] GetPartyMembers()
        {
            return  PartyLookup.GetParty()
                .Where(x => x.transform != transform)
                .ToArray();
        }

        private void PartySelectionHandling()
        {
            var partyMembers = GetPartyMembers();
            
            if (Input.GetKeyDown(KeyCode.F1)) if (partyMembers.Length > 0) 
                _targetingComponent.CommandSetTarget(partyMembers[0]);
            
            if (Input.GetKeyDown(KeyCode.F2)) if (partyMembers.Length > 1) 
                _targetingComponent.CommandSetTarget(partyMembers[1]);
            
            if (Input.GetKeyDown(KeyCode.F3)) if (partyMembers.Length > 2) 
                _targetingComponent.CommandSetTarget(partyMembers[2]);
            
            if (Input.GetKeyDown(KeyCode.F4)) if (partyMembers.Length > 3) 
                _targetingComponent.CommandSetTarget(partyMembers[3]);
        }

        private void EscapeHandling()
        {
            if (!Input.GetKeyDown(KeyCode.Escape)) return;

            if (_targetingComponent.Target != null) _targetingComponent.CommandSetTarget(null);
        }

        private void TargetHandling()
        {
            var hover = GetHover();
            var leftClick = Input.GetMouseButtonDown(0);
            var rightClick = Input.GetMouseButtonDown(1);

            _targetingComponent.CommandSetHoverTarget(hover);
            if (rightClick && hover != null)
            {
                _targetingComponent.CommandSetTarget(hover);
                _attackingComponent.IsAttacking = true;
            }
            else if (leftClick) _targetingComponent.CommandSetTarget(hover);

            var tab = Input.GetKeyDown(KeyCode.Tab);
            if (tab)
            {
                if (enemiesOnScreen.Any())
                {
                    var currentTargetIndex = enemiesOnScreen.IndexOf(_targetingComponent.Target);

                    if (currentTargetIndex >= 0) currentTargetIndex = (currentTargetIndex + 1) % enemiesOnScreen.Count;
                    else currentTargetIndex = 0;
                
                    _targetingComponent.CommandSetTarget(enemiesOnScreen[currentTargetIndex]);
                } else _targetingComponent.CommandSetTarget(null);
            }
        }

        private GameObject GetHover()
        {
            var position = (Vector2)_camera.ScreenToWorldPoint(Input.mousePosition);

            var size = Physics2D.RaycastNonAlloc(position, Vector2.zero, _rayCastHits, 0f);
            if (size == 0) return null;
            
            var orderedHits = _rayCastHits.Take(size)
                .Where(x => x.collider.CompareTag("Hitbox"))
                .OrderBy(x => Vector2.Distance(position, x.collider.bounds.center))
                .ToArray();
            if (!orderedHits.Any()) return null;

            var hit = orderedHits.First().collider;
            if (hit == null) return null;

            var parentGameObject = hit.transform.parent.gameObject;
            return parentGameObject;
        }

        public KeyBinding[] GetSpellBindings()
        {
            return _spellBindings?.ToArray();
        }

        public void ServerInitialize(Character character)
        {
            _spellBindings.Clear();
            _spellBindings.AddRange(character.spellBindings);
        }

        public void RegisterInteractables(IEnumerable<GameObject> interactables)
        {
            foreach (var interactable in interactables) 
                if (!_interactables.Contains(interactable)) 
                    _interactables.Add(interactable);
            
            InteractableEventHandler.InvokeOnInteractablesChanged(gameObject, _interactables.Count);
        }

        public void UnregisterInteractables(IEnumerable<GameObject> interactables)
        {
            foreach (var interactable in interactables) 
                if (_interactables.Contains(interactable)) 
                    _interactables.Remove(interactable);
            
            InteractableEventHandler.InvokeOnInteractablesChanged(gameObject, _interactables.Count);
        }

        public IEnumerable<GameObject> GetNearbyEnemies()
        {
            return enemiesOnScreen;
        }
    }
}