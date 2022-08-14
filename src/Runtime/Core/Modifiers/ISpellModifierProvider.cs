using System.Collections.Generic;
using MyRpg.Core.Models;

namespace MyRpg.Core.Modifiers
{
    public interface ISpellModifierProvider
    {
        IEnumerable<SpellModifier> GetModifiers(Spell spell);
    }
}