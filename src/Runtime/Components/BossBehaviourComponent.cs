using System.Linq;
using Mirror;
using MyRpg.Core.Components;
using MyRpg.Core.Models;
using UnityEngine;

namespace MyRpg.Components
{
    public class BossBehaviourComponent : NetworkBehaviour, IBossBehaviourComponent
    {
        [SerializeField] private BossFight bossFight;

        private IProgressComponent _progressComponent;
        private ICastingComponent _castingComponent;
        private ITargetingComponent _targetingComponent;
        private IThreatComponent _threatComponent;

        private bool _inCombat;
        public bool DoNormalBehaviour() => !_bossAbilityInProgress;
        private bool _bossAbilityInProgress;
        private INavigatingMovementComponent _movementComponent;

        private void Awake()
        {
            _progressComponent = GetRequiredComponent<IProgressComponent>();
            _castingComponent = GetRequiredComponent<ICastingComponent>();
            _targetingComponent = GetRequiredComponent<ITargetingComponent>();
            _threatComponent = GetRequiredComponent<IThreatComponent>();
            _movementComponent = GetRequiredComponent<INavigatingMovementComponent>();
        }

        [Server]
        public void ServerExecuteBossBehaviour()
        {
            if (_targetingComponent.Target == null)
            {
                if (_inCombat) Reset();
                return;
            }

            _inCombat = true;
            
            foreach (var bossAbility in bossFight.BossAbilities) AbilityBehaviour(bossAbility);
        }

        private void Reset()
        {
            foreach (var bossAbility in bossFight.BossAbilities) bossAbility.WaitTime = null;
            _inCombat = false;
        }

        private void AbilityBehaviour(BossAbility bossAbility)
        {
            if (bossAbility.WaitTime == null) bossAbility.WaitTime = bossAbility.CastingDelay;
            else bossAbility.WaitTime -= Time.deltaTime;
            
            if (_bossAbilityInProgress || bossAbility.WaitTime > 0) return;

            _progressComponent.ServerCancel();
            _movementComponent.ServerStop();
            
            _progressComponent.ServerStartProgress(
                bossAbility.Spell.CastTime, 
                () => AbilityCompleted(bossAbility), 
                bossAbility.Spell.Name, 
                bossAbility.Spell.Interruptible);
            
            if (bossAbility.InteractOnStart != null) 
                bossAbility.InteractOnStart.GetComponent<IInteractableComponent>().ServerInteract();

            _bossAbilityInProgress = true;

            bossAbility.WaitTime = bossAbility.CastingPeriod;
        }

        private void AbilityCompleted(BossAbility bossAbility)
        {
            GameObject target = null;
            if (bossAbility.TargetRandomPlayer) target = TargetRandomPlayer();
            else target = _targetingComponent.Target;

            _castingComponent.ServerExecuteSpell(new SpellCasting
            {
                Caster = gameObject,
                SpellId = bossAbility.Spell.Id,
                Target = target
            });
            
            if (bossAbility.InteractOnFinish != null) 
                bossAbility.InteractOnFinish.GetComponent<IInteractableComponent>().ServerInteract();

            _bossAbilityInProgress = false;
        }

        private GameObject TargetRandomPlayer()
        {
            var targets = _threatComponent.GetAllTargets();
            if (targets.Count == 0) return null;
            
            var i = Random.Range(0, targets.Count);
            return targets[i];
        }
    }
}