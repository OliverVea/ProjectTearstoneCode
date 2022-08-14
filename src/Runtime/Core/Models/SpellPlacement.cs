using System;
using UnityEngine;

namespace MyRpg.Core.Models
{
    public enum SpellPlacementType { Circle, Cone }
    
    
    [CreateAssetMenu(menuName = "Spells/Create Placement")]
    [Serializable]
    public class SpellPlacement : ScriptableObject
    {
        [field: SerializeField] public SpellPlacementType PlacementType { get; private set; }

        // Circle
        [field: SerializeField] public bool CircleCenteredOnPlayer { get; private set; }
        [field: SerializeField] public float CircleRadius { get; private set; }
        [field: SerializeField] public Vector2 CircleCenter { get; set; }
        
        // Cone
        [field: SerializeField] public float ConeAngle { get; private set; }
        [field: SerializeField] public float ConeRange { get; private set; }
        [field: SerializeField] public Vector2 ConeDirection { get; set; }
    }
}