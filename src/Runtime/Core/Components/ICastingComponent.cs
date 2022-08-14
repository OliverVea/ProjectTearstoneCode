using System.Collections.Generic;
using MyRpg.Core.Models;
using UnityEngine;

namespace MyRpg.Core.Components
{
    public interface ICastingComponent : IBase
    {
        List<Spell> AvailableSpells { get; }
        bool CanCastSpell(string spellId, GameObject target);
        void CommandCast(string spellId, GameObject target);
        void ServerCast(string spellId, GameObject target);
        void ServerExecuteSpell(SpellCasting spellCasting);
    }
}