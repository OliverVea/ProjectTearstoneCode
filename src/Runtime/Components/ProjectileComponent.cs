using Mirror;
using MyRpg.Core;
using MyRpg.Core.Components;
using MyRpg.Core.Constants;
using UnityEngine;

namespace MyRpg.Components
{
    public class ProjectileComponent : NetworkBehaviour, IProjectileComponent
    {
        [SerializeField] private bool waitForHitAnimation;
        [SerializeField] private GameObject _target;
        
        private string _effectId;
        private GameObject _source;
        private bool _initialized;

        private IProjectileMovementComponent _movement;

        private Vector3 TargetPosition => _target.transform.position + Vector3.up * 1f;

        private void Awake()
        {
            _movement = GetComponent<IProjectileMovementComponent>();
        }

        public void Initialize(string effectId, GameObject source, GameObject target)
        {
            _movement.SetTarget(target.transform);
            
            _effectId = effectId;
            _target = target;
            _source = source;
            _initialized = true;
        }
        
        private void Update()
        {
            if (!_initialized) return;

            var remainingDistance = _movement.RemainingDistance();
            var hitRange = ConstantValues.ServerProjectileHitRange;
            var inRange = remainingDistance < hitRange;
            if (!inRange) return;

            if (waitForHitAnimation)
            {
                GetComponent<Animator>()?.SetTrigger("Impact");
                return;
            }

            Destroy();
            ApplyEffect();
        }

        private void ApplyEffect()
        {
            if (!NetworkServer.active) return;
            var targetEffects = _target.transform.GetComponent<IEffectsComponent>();
            targetEffects?.ServerApplyEffect(_source, _effectId);
        }

        private void Destroy()
            => Destroy(gameObject);

        public void AnimationApplyEffect()
            => ApplyEffect();

        public void AnimationDestroy()
            => Destroy();
    }
}
