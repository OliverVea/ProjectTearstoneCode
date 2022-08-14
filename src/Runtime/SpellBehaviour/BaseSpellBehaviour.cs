using System.Collections.Generic;
using System.Linq;
using MyRpg.Core;
using MyRpg.Core.Components;
using MyRpg.Core.Constants;
using UnityEngine;

namespace MyRpg.SpellBehaviour
{
    public abstract class BaseSpellBehaviour : ISpellBehaviour
    {
        public abstract GameObject GetTargetForSpell(GameObject caster, string spellId, GameObject target);

        protected IEnumerable<GameObject> GetAlliesInRange(ICastingComponent castingComponent, string spellId)
        {
            var statusComponent = castingComponent.GetComponent<IStatusComponent>();
            return GameObject.FindGameObjectsWithTag("Character")
                .Where(character => OwnFaction(statusComponent, character))
                .Where(character => castingComponent.CanCastSpell(spellId, character));
        }


        private static bool OwnFaction(IStatusComponent statusComponent, GameObject character)
        {
            var characterStatusComponent = character.GetComponent<IStatusComponent>();
            if (characterStatusComponent == null) return false;
            var isFriendly = FactionRelations.IsFriendly(
                statusComponent.GetFaction(), 
                characterStatusComponent.GetFaction());
            return isFriendly;
        }
    }
}