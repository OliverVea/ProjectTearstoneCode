using System.Collections.Generic;
using UnityEngine;

namespace MyRpg.UI.UnitFrame.Bars
{
    [CreateAssetMenu(menuName = "UI/Create Color")]
    public class BarColor : ScriptableObject
    {
        [field: SerializeField] public List<float> Thresholds { get; private set; }
        [field: SerializeField] public List<Color> Colors { get; private set; }
    }
}