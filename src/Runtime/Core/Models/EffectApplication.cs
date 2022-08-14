using System;
using UnityEngine;

namespace MyRpg.Core.Models
{
    [Serializable]
    public class EffectApplication
    {
        public Effect Effect => LookupComponent.GetEffect(effectId);
        public string effectId;
        public float timeActive;
        public float absorbedDamage;
        public GameObject owner;
    }
}