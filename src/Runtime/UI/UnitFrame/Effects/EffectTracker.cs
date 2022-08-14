using MyRpg.Core.Components;
using MyRpg.Core.Models;
using MyRpg.UI.Tooltips;
using UnityEngine;
using UnityEngine.UI;

namespace MyRpg.UI.UnitFrame.Effects
{
    public class EffectTracker : MonoBehaviour
    {
        [SerializeField] private Image trackerImage;
        [SerializeField] private RectTransform durationCoverTransform;

        private IEffectsComponent _effectsComponent;
        private string _effectId;

        private void Update()
        {
            if (_effectId == null) return;

            var effectApplication = _effectsComponent.GetEffectApplication(_effectId);
            if (effectApplication == null) return;

            SetDuration(effectApplication.timeActive, effectApplication.Effect.Duration);
        }

        private void SetDuration(float timeActive, float duration)
        {
            var ratio = Mathf.Clamp01(timeActive / duration);
            durationCoverTransform.localScale = new Vector3(1, ratio, 1);
        }

        public void Initialize(IEffectsComponent effectsComponent, string effectId)
        {
            _effectsComponent = effectsComponent;
            _effectId = effectId;

            var effect = LookupComponent.GetEffect(_effectId);
            trackerImage.sprite = effect.Icon;

            GetComponent<EffectTooltipTriggerComponent>()?.Initialize(effect);
        }

        public float GetRemainingDuration()
        {
            if (_effectId == null) return float.MaxValue;
            
            var effectApplication = _effectsComponent.GetEffectApplication(_effectId);
            if (effectApplication == null) return float.MaxValue;

            return effectApplication.Effect.Duration - effectApplication.timeActive;
        }
    }
}