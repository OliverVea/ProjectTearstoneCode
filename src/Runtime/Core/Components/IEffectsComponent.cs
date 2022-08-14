using System.Collections.Generic;
using MyRpg.Core.Models;
using UnityEngine;

namespace MyRpg.Core.Components
{
    public interface IEffectsComponent : IBase
    {
        public void ServerApplyEffect(GameObject owner, string effectId);
        void ServerRemoveEffect(string effectId);

        bool IsStunned();
        public float GetMovementSpeedModifier();

        public EffectApplication GetEffectApplication(string effectId);
        public Sprite GetIconFromId(string effectId);
        IEnumerable<EffectApplication> GetEffectApplications();
    }
}