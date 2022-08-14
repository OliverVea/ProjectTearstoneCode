using System.Collections.Generic;
using System.Globalization;
using MyRpg.Core.Models;
using UnityEngine;

namespace MyRpg.UI.Tooltips
{
    public class SpellTooltipTriggerComponent : AbstractTooltipTriggerComponent
    {
        [SerializeField] private Spell spell;

        public void SetSpell(Spell newSpell)
        {
            spell = newSpell;
        }
        
        protected override string GetTitle()
        {
            return spell.Name;
        }

        protected override (string, string)[] GetAttributes()
        {
            var attributes = new List<(string, string)>();
            
            attributes.Add(("Mana cost", spell.ManaCost.ToString(CultureInfo.InvariantCulture)));
            if (spell.Cooldown > 0f) attributes.Add(("Cooldown", spell.Cooldown.ToString(CultureInfo.InvariantCulture) + "s"));
            attributes.Add(("Range", spell.Range.ToString(CultureInfo.InvariantCulture)));

            return attributes.ToArray();
        }

        protected override string GetText()
        {
            return spell.Description;
        }
    }
}