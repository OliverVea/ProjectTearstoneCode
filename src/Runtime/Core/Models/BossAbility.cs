using System;
using UnityEngine;

namespace MyRpg.Core.Models
{
    [Serializable]
    public class BossAbility
    {
        [field: SerializeField] public Spell Spell { get; private set; }
        [field: SerializeField] public GameObject InteractOnStart { get; private set; }
        [field: SerializeField] public GameObject InteractOnFinish { get; private set; }
        [field: SerializeField] public float CastingPeriod { get; private set; }
        [field: SerializeField] public float CastingDelay { get; private set; }
        [field: SerializeField] public bool TargetRandomPlayer { get; private set; }
        
        public float? WaitTime;
    }
}