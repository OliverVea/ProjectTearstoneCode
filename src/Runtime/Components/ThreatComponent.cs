using System.Collections.Generic;
using System.Linq;
using Mirror;
using MyRpg.Core.Components;
using MyRpg.Core.Constants;
using MyRpg.Core.Events;
using MyRpg.Core.Models;
using MyRpg.Core.Modifiers;
using MyRpg.Core.StaticProviders;
using UnityEngine;

namespace MyRpg.Components
{
    [RequireComponent(typeof(ITargetingComponent))]
    public partial class ThreatComponent : NetworkBehaviour, IThreatComponent, ICombatProvider
    {
        [SerializeField] private List<ThreatEntry> threatTable;

        private IHealthEventHandler _healthEventHandler;
        private IDeathEventHandler _deathEventHandler;
        
        private ITargetingComponent _targetingComponent;
        private IStatusComponent _statusComponent;
        
        private ITauntedProvider[] _tauntedProviders;

        private void Awake()
        {
            _healthEventHandler = GetRequiredComponent<IHealthEventHandler>();
            _deathEventHandler = GetRequiredComponent<IDeathEventHandler>();
            
            _targetingComponent = GetRequiredComponent<ITargetingComponent>();
            _statusComponent = GetRequiredComponent<IStatusComponent>();
            
            _tauntedProviders = GetComponents<ITauntedProvider>();
        }
        
        private void OnEnable()
        {
            _healthEventHandler.RegisterOnAllHealthChanged(OnHealthChanged);
            _deathEventHandler.RegisterAllOnDeath(OnDeath);
            CombatProvider.RegisterProvider(this);
        }

        private void OnDisable()
        {
            _healthEventHandler.UnregisterOnAllHealthChanged(OnHealthChanged);
            _deathEventHandler.UnregisterAllOnDeath(OnDeath);
            CombatProvider.UnregisterProvider(this);
        }

        private void OnDeath(IDeathComponent source)
        {
            if (!isServer || source == null) return;
            if (source.transform == transform)
            {
                threatTable = new List<ThreatEntry>();
                return;
            }
            
            ServerDropThreat(source.gameObject);
        }

        private void OnHealthChanged(GameObject eventSource, HealthChangedData data)
        {
            if (!isServer || data.Source == null) return;
            
            // If this character was damaged by the source.
            if (data.Change < 0 && eventSource.transform == transform) 
                ServerAddThreat(data.Source, Mathf.Abs(data.Change) * ConstantValues.ThreatPerDamage);
            
            // If a character this character is engaged in combat with is healed by the source.
            if (data.Change > 0 && threatTable.Any(x => x.target.transform == eventSource.transform))
                ServerAddThreat(data.Source, Mathf.Abs(data.Change) * ConstantValues.ThreatPerHealing);
        }

        [ServerCallback]
        private void Update()
        {
            if (!threatTable.Any())
            {
                _targetingComponent.ServerSetTarget(null);
                return;
            }

            var taunter = _tauntedProviders.Select(x => x.GetTaunter()).FirstOrDefault();

            if (taunter != null)
            {
                _targetingComponent.ServerSetTarget(taunter);
                PullAlliesNearby();
                return;
            }

            var target = threatTable.OrderByDescending(x => x.threat).First().target;
            _targetingComponent.ServerSetTarget(target);
            PullAlliesNearby();
        }

        private void PullAlliesNearby()
        {
            var characters = GameObject.FindGameObjectsWithTag("Character");

            foreach (var character in characters)
            {
                var characterStatus = character.GetComponent<IStatusComponent>();

                var isFriendly = FactionRelationsProvider.IsFriendly(transform, character.transform);
                
                if (characterStatus.IsDead() || isFriendly) continue;

                var range = (transform.position - character.transform.position).magnitude;
                if (range <= ConstantValues.ThreatPropagationRange)
                {
                    var characterThreatComponent = character.GetComponent<IThreatComponent>();
                    if (characterThreatComponent == null) return;
                    characterThreatComponent.ServerPull(_targetingComponent.Target);
                }
            }
        }

        [Server]
        public void ServerAddThreat(GameObject target, float threat)
        {
            var targetStatus = target.GetComponent<IStatusComponent>();
            if (targetStatus == null || targetStatus.IsDead()) return;

            var isFriendly = !FactionRelationsProvider.IsAttackable(transform, target.transform);
            if (isFriendly) return;
            
            var entry = GetEntryForTarget(target);

            if (entry == null) ServerInitializeThreat(target, threat);
            else entry.threat += threat;
        }

        private void ServerInitializeThreat(GameObject target, float initialThreat)
        {
            if (!threatTable.Any()) initialThreat += ConstantValues.ThreatOnPull;
            threatTable.Add(new ThreatEntry {target = target, threat = initialThreat});
        }

        public void ServerDropThreat(GameObject target)
        {
            if (threatTable.All(x => x.target.transform != target.transform)) return;
            Debug.Log($"{gameObject} dropping threat for {target}.");
            threatTable.RemoveAll(x => x.target == target);
        } 

        public void ServerTaunt(GameObject owner)
        {
            var entry = GetEntryForTarget(owner);

            if (entry == null)
            {
                entry = new ThreatEntry { target = owner };
                threatTable.Add(entry);
            }

            var maxThreat = threatTable.Max(x => x.threat);
            entry.threat = maxThreat + ConstantValues.ThreatOnTaunt;
        }

        public void ServerPull(GameObject target)
        {
            if (threatTable.Any()) return;
            
            threatTable.Add(new ThreatEntry
            {
                target = target,
                threat = ConstantValues.ThreatOnPull
            });
        }

        public List<GameObject> GetAllTargets()
        {
            return threatTable.Where(x => x.threat > 0).Select(x => x.target).ToList();
        }

        ThreatEntry GetEntryForTarget(GameObject target)
            => threatTable.FirstOrDefault(x => x.target.transform == target.transform);

        public void ServerInitialize(Character messageCharacter) { }
        
        public bool IsInCombat(GameObject character)
        {
            return threatTable.Any(x => x.target.transform == character.transform);
        }
    }
}