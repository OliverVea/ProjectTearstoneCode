using MyRpg.Core.Animation;
using MyRpg.Core.Components;
using UnityEngine;

namespace MyRpg.Components
{
    public class AnimationTargetComponent : MonoBehaviour, IAnimationTargetComponent
    {
        public void PlayEffect(GameObject effect, string effectId)
        {
            var effectGameObject = Instantiate(effect, transform.position, Quaternion.identity);
            effectGameObject.GetComponent<IEffectAnimationComponent>().SetParent(transform, effectId);
        }
    }
}