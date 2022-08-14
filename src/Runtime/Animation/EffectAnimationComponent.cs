using MyRpg.Core.Animation;
using MyRpg.Core.Components;
using UnityEngine;

namespace MyRpg.Animation
{
    [ExecuteInEditMode]
    public class EffectAnimationComponent : MonoBehaviour, IEffectAnimationComponent
    {
        [SerializeField] private bool hasExitAnimation;
        [SerializeField] private Vector2 offset;
        [SerializeField] private Transform target;
        
        private Animator _animator;
        private IEffectsComponent _targetEffectsComponent;
        private string _effectId;

        void Start()
        {
            if (!Application.isPlaying) return;
            
            _animator = GetComponent<Animator>();
            if (!hasExitAnimation) Destroy(gameObject, _animator.GetCurrentAnimatorStateInfo(0).length );
        }

        // Animation event
        void Destroy()
        {
            Destroy(gameObject);
        }

        private void Update()
        {
            if (transform != null && target != null) transform.position = target.position + (Vector3)offset;
            
            if (!Application.isPlaying) return;
            var effectApplication = _targetEffectsComponent.GetEffectApplication(_effectId);
            if (effectApplication == null) _animator.SetTrigger("Removal");
        }

        public void SetParent(Transform parent, string effectId)
        {
            target = parent;
            _targetEffectsComponent = target.GetComponent<IEffectsComponent>();
            _effectId = effectId;
        }
    }
}