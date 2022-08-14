using UnityEngine;

namespace MyRpg.Core.StaticProviders
{
    public interface ICombatProvider
    {
        bool IsInCombat(GameObject character);
    }
}