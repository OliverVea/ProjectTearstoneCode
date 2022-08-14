using UnityEngine;

namespace MyRpg.Core.Components
{
    public interface IAnimationTargetComponent
    {
        void PlayEffect(GameObject effect, string effectId);
    }
}