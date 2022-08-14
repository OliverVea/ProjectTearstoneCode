using System;

namespace MyRpg.Core.Models
{
    [Serializable]
    public class SpellModifier
    {
        public string SpellId;
        
        public float CastTimeAbsolute;
        public float DurationAbsolute;
        public float DamageAbsolute;

        public float CastTimeRelative = 1f;
        public float DurationRelative = 1f;
        public float DamageRelative = 1f;
    }
}