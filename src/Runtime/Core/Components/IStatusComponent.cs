using MyRpg.Core.Constants;
using MyRpg.Core.Models;
using UnityEngine;

namespace MyRpg.Core.Components
{
    public interface IStatusComponent : IBase
    {
        bool IsDead();
        Faction GetFaction();
        bool CanMove();
        bool CanAttack();
        float GetAttackRange();
        float GetMovementSpeed();
        Sprite GetPortrait();
        string GetName();
        float GetDamageReduction();
        Class GetClass();
        float GetHealthForClass(Class characterCharacterClass);
        float GetManaForClass(Class characterCharacterClass);
        float GetKillExperience();
        string GetCharacterId();
    }
}