using System;
using UnityEngine;

namespace MyRpg.Core.Models
{
    [Serializable]
    public struct KeyBinding
    {
        public KeyCode key;
        public string spellId;
    }
}