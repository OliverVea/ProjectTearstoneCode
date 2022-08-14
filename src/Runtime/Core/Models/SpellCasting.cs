using UnityEngine;

namespace MyRpg.Core.Models
{
    public class SpellCasting
    {
        public string SpellId;
        public GameObject Caster;
        public GameObject Target;

        public Spell Spell => LookupComponent.GetSpell(SpellId);
        public bool IsChanneled => Spell.IsChanneled;
        public bool IsInstant => Spell.CastTime <= 0 && !Spell.IsChanneled;
        public bool IsCasted => Spell.CastTime > 0 && !Spell.IsChanneled;
        public bool IsAreaOfEffect => Spell.IsAreaOfEffect;
        public bool IsProjectile => Spell.Projectile != null;
    }
}