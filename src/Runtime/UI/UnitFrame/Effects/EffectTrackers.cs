using System.Collections.Generic;
using System.Linq;
using MyRpg.Core.Models;
using UnityEngine;

namespace MyRpg.UI.UnitFrame.Effects
{
    public abstract class EffectTrackers : UiElement
    {
        [SerializeField] private GameObject effectTrackerPrefab;
        [SerializeField] private Transform gridLayoutTransform;
        
        private readonly Dictionary<string, EffectTracker> _effectTrackers = new Dictionary<string, EffectTracker>();

        protected void AddEffect(string effectId)
        {
            if (effectTrackerPrefab == null || _effectTrackers.ContainsKey(effectId)) return;
            var effect = LookupComponent.GetEffect(effectId);
            if (effect == null || effect.IsHidden) return;

            var effectTrackerObject = Instantiate(effectTrackerPrefab, gridLayoutTransform);
            var effectTracker = effectTrackerObject.GetComponent<EffectTracker>();
            InitializeEffectTracker(effectTracker, effectId);
            _effectTrackers.Add(effectId, effectTracker);
            SortEffects();
        }

        protected virtual void InitializeEffectTracker(EffectTracker effectTracker, string effectId)
        {
            
        }

        protected void RemoveEffect(string effectId)
        {
            if (!_effectTrackers.ContainsKey(effectId)) return;
            Destroy(_effectTrackers[effectId].gameObject);
            _effectTrackers.Remove(effectId);
        }

        protected void ClearEffects()
        {
            var effectIds = _effectTrackers.Select(x => x.Key).ToList();
            foreach (var effectId in effectIds) RemoveEffect(effectId);
        }

        private void SortEffects()
        {
            if (_effectTrackers.Count <= 1) return;
            
            var sortedEffectTrackers =
                _effectTrackers.OrderBy(x => x.Value.GetRemainingDuration()).ToArray();

            for (int i = 0; i < sortedEffectTrackers.Length; i++)
            {
                var guid = sortedEffectTrackers[i].Key;
                _effectTrackers[guid].transform.SetSiblingIndex(i);
            }
        }


    }
}