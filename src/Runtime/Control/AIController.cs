using System.Linq;
using Mirror;
using MyRpg.Components;
using MyRpg.Core;
using MyRpg.Core.Components;
using MyRpg.Core.Models;
using MyRpg.Core.StaticProviders;
using MyRpg.SpellBehaviour;
using UnityEngine;
using Random = UnityEngine.Random;

namespace MyRpg.Runtime.Control
{
    internal enum AIState { Idle, Fighting, Returning }
    
    public class AIController : NetworkBehaviour, IAIController
    {
        [SerializeField] private float _aggroRange = 5f;
        [SerializeField] private float _maxAggroRange = float.MaxValue;
        [SerializeField] private float _maxChaseRange = float.MaxValue;
        [SerializeField] private float _secondsBetweenUpdates = 0.15f;

        private ITargetingComponent _targetingComponent;
        private INavigatingMovementComponent _movementComponent;
        private IStatusComponent _statusComponent;
        private IAttackingComponent _attackingComponent;
        private IThreatComponent _threatComponent;
        private ICastingComponent _castingComponent;
        private IBossBehaviourComponent _bossBehaviourComponent;
        
        [SerializeField] private AIState _state;
        
        private Vector3 _lastIdlePosition;
        private float _lastUpdate = 10f;
        
        [Header("Patrol")]
        [SerializeField] private int patrolStartingIndex;
        [SerializeField] private PatrolPathComponent _patrolPathComponent;
        private PatrolProgress _patrolProgress;

        private void Awake()
        {
            _targetingComponent = GetRequiredComponent<ITargetingComponent>();
            _movementComponent = GetRequiredComponent<INavigatingMovementComponent>();
            _statusComponent = GetRequiredComponent<IStatusComponent>();
            _attackingComponent = GetRequiredComponent<IAttackingComponent>();
            _threatComponent = GetRequiredComponent<IThreatComponent>();
            _castingComponent = GetRequiredComponent<ICastingComponent>();
            _bossBehaviourComponent = GetComponent<IBossBehaviourComponent>();

            if (_patrolPathComponent != null) 
                _patrolProgress = new PatrolProgress(_patrolPathComponent, patrolStartingIndex);
        }

        private void Start()
        {
            if (_patrolProgress != null) 
                _movementComponent.ServerSetTarget(_patrolProgress.Waypoint, _patrolProgress.MovementSpeed);
        }

        [ServerCallback]
        private void Update()
        {
            if (_statusComponent.IsDead()) return;
                
            _lastUpdate += Time.deltaTime;

            if (_bossBehaviourComponent != null)
            {
                _bossBehaviourComponent.ServerExecuteBossBehaviour();
                if (!_bossBehaviourComponent.DoNormalBehaviour()) return;
            };

            if (_lastUpdate <= _secondsBetweenUpdates) return;
            
            if (_state == AIState.Idle) IdleBehaviour();
            if (_state == AIState.Fighting) FightingBehaviour();
            if (_state == AIState.Returning) ReturningBehaviour();
                
            _lastUpdate = -_secondsBetweenUpdates * Random.value;
        }

        private bool BossBehaviour()
        {
            throw new System.NotImplementedException();
        }

        private void IdleBehaviour()
        {
            _lastIdlePosition = transform.position;

            if (_targetingComponent.Target != null)
            {
                Debug.Log("Now fighting.");
                _state = AIState.Fighting;
                return;
            }
            
            var target = CheckIfAnyTargetsInRange();

            if (target != null)
            {
                _threatComponent.ServerPull(target);
                return;
            }

            if (_patrolProgress == null) return;

            if (_movementComponent.IsMoving) return;

            if (_patrolProgress.TimeWaited <= _patrolProgress.ToWait)
            {
                _patrolProgress.TimeWaited += _lastUpdate;
                return;
            }

            _patrolProgress.TimeWaited = 0;
            _patrolProgress.NextWaypoint();
            _movementComponent.ServerSetTarget(_patrolProgress.Waypoint, _patrolProgress.MovementSpeed);
        }
        
        private GameObject CheckIfAnyTargetsInRange()
        {
            var characters = GameObject.FindGameObjectsWithTag("Character");

            foreach (var character in characters)
                if (CheckCharacterInRange(character)) return character;

            return null;
        }

        private bool CheckCharacterInRange(GameObject character)
        {
            var delta = transform.position - character.transform.position;
            if (delta.x > _aggroRange || delta.y > _aggroRange) return false;

            if (!FactionRelationsProvider.IsAttackable(transform, character.transform)) return false;

            var statusComponent = character.GetComponent<IStatusComponent>();
            if (statusComponent.IsDead()) return false;
            
            if (!HasLineOfSight(character)) return false;

            var range = delta.magnitude;
            return range <= _aggroRange;
        }
        
        private bool HasLineOfSight(GameObject target)
        {
            var hits = Physics2D.LinecastAll(transform.position, target.transform.position);
            return !hits.Any(hit => hit.transform.gameObject.CompareTag("Wall"));
        }

        private void FightingBehaviour()
        {
            _attackingComponent.IsAttacking = true;
            
            var target = _targetingComponent.Target;

            if (target == null)
            {
                Debug.Log($"{gameObject} has no remaining targets. Returning.");
                _state = AIState.Returning;
                _movementComponent.ServerSetTarget(_lastIdlePosition);
                return;
            }
            
            var aiPosition = transform.position;
            var targetPosition = target.transform.position;
            var targetStatus = target.GetComponent<IStatusComponent>();

            if (ShouldDropAggro(aiPosition, targetPosition, targetStatus))
            {
                _threatComponent.ServerDropThreat(target);
                return;
            }

            var spellCasting = GetSpellToCast();
            if (spellCasting != null)
            {
                _movementComponent.ServerStop();
                _castingComponent.ServerCast(spellCasting.Spell.Id, spellCasting.Target);
                return;
            }

            if (_attackingComponent.IsInRange())
            {
                _movementComponent.ServerStop();
                return;
            }
            
            _movementComponent.ServerSetTarget(target.transform.position);
        }

        private bool ShouldDropAggro(Vector3 aiPosition, Vector3 targetPosition, IStatusComponent targetStatus)
        {
            if ((targetPosition - aiPosition).magnitude > _maxAggroRange) return true;
            if ((_lastIdlePosition - aiPosition).magnitude > _maxChaseRange) return true;
            if (targetStatus.IsDead()) return true;

            return false;
        }

        private SpellCasting GetSpellToCast()
        {
            var randomizedOrder = _castingComponent.AvailableSpells.OrderBy(_ => Random.value);
            foreach (var spell in randomizedOrder)
            {
                var spellBehaviour = SpellBehaviours.GetBehaviour(spell.Id);
                if (spellBehaviour == null) continue;
                
                var target = spellBehaviour.GetTargetForSpell(gameObject, spell.Id, _targetingComponent.Target);
                if (target == null) continue;
                
                var canCastSpell = _castingComponent.CanCastSpell(spell.Id, target);
                if (!canCastSpell) continue;
                
                return new SpellCasting
                {
                    Caster = gameObject,
                    SpellId = spell.Id,
                    Target = target
                };
            }

            return null;
        }

        private void ReturningBehaviour()
        {
            if (_movementComponent.IsMoving) return;
            
            Debug.Log("Now idling.");
            _state = AIState.Idle;
        }
        
        private void OnDrawGizmosSelected()
        {
            var center = transform.position;
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(center, _aggroRange);
        }

        public void SetPatrolPath(IPatrolPathComponent newPatrolPathComponent, int startingWaypointIndex)
        {
            _patrolProgress = new PatrolProgress(newPatrolPathComponent, startingWaypointIndex);
            _movementComponent.ServerSetTarget(_patrolProgress.Waypoint, _patrolProgress.MovementSpeed);
        }
    }
}