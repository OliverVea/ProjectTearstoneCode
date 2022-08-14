using UnityEngine;

namespace MyRpg.Core.Animation
{
    public interface IEffectAnimationComponent
    {
        void SetParent(Transform transform, string effectId);
    }
}