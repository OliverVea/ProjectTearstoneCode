using System;
using System.Collections.Generic;
using UnityEngine;

namespace MyRpg.Core.Models
{
    [Serializable]
    public class BossFight
    {
        [field: SerializeField] public float EnrageTimer { get; private set; }
        [field: SerializeField] public List<BossAbility> BossAbilities { get; private set; }
    }
}