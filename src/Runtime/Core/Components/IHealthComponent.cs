using UnityEngine;

namespace MyRpg.Core.Components
{
    public interface IHealthComponent : IBase
    {
        float GetMaxHealth();
        float GetMissingHealth();
        float GetCurrentHealth();
        void ServerTakeDamage(GameObject source, float weaponDamage);
        void ServerTakeHealing(GameObject source, float healing);
    }
}