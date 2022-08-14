using System;
using UnityEngine;

namespace MyRpg.Core.Models
{
    [Serializable]
    public class PylonAbility
    {
        [field: SerializeField] public GameObject Interact { get; private set; }
    }
}