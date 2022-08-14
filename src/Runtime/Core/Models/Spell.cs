using System;
using System.Collections.Generic;
using MyRpg.Core.Constants;
using UnityEngine;

namespace MyRpg.Core.Models
{
    [CreateAssetMenu(menuName = "Spells/Create Spell")]
    [Serializable]
    public class Spell : ScriptableObject
    {
        [field: SerializeField] public string Id { get; private set; }
        [field: SerializeField] public string Name { get; private set; }
        [field: SerializeField] public string Description { get; private set; }
        [field: SerializeField] public Sprite Icon { get; private set; }
        [field: SerializeField] public GameObject Projectile { get; private set; }
        
        # region Targeting
        [field: SerializeField] public bool CanTargetSelf { get; private set; }
        [field: SerializeField] public bool CanTargetFriendly { get; private set; }
        [field: SerializeField] public bool CanTargetHostile { get; private set; }
        [field: SerializeField] public bool TargetsDead { get; private set; }
        # endregion

        # region Spell Attributes
        [field: SerializeField] public float CastTime { get; private set; }
        [field: SerializeField] public float ManaCost { get; private set; }
        [field: SerializeField] public float Range { get; private set; } = ConstantValues.DefaultSpellRange;
        [field: SerializeField] public float Cooldown { get; private set; }
        [field: SerializeField] public string[] SpellEffectIds { get; private set; }
        [field: SerializeField] public List<EffectType> Dispels { get; private set; }
        [field: SerializeField] public bool Interruptible { get; private set; }
        [field: SerializeField] public bool IsAreaOfEffect { get; private set; }
        [field: SerializeField] public bool IsChanneled { get; private set; }
        [field: SerializeField] public bool IsCharge { get; private set; }
        [field: SerializeField] public SpellPlacement SpellPlacement { get; private set; }

        # endregion

    }
}