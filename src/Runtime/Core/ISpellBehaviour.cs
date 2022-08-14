using UnityEngine;

namespace MyRpg.Core
{
    public interface ISpellBehaviour
    {
        GameObject GetTargetForSpell(GameObject caster, string spellId, GameObject target);
    }
}