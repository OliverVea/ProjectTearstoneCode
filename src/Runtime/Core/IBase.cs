using MyRpg.Core.Models;
using UnityEngine;
// ReSharper disable InconsistentNaming

namespace MyRpg.Core
{
    public interface IBase
    {
        Transform transform { get; }
        GameObject gameObject { get; }
        
        T GetComponent<T>();
        T GetRequiredComponent<T>();
        
        void ServerInitialize(Character character);
    }
}