using UnityEngine;

namespace MyRpg.Core.Components
{
    public interface IProjectileComponent
    {
        void Initialize(string effectId, GameObject source, GameObject target);
    }
}
