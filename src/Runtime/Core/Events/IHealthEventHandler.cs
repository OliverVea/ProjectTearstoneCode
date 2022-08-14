using System;
using UnityEngine;

namespace MyRpg.Core.Events
{
    public class HealthChangedData
    {
        public GameObject Source;
        public float NewHealth;
        public float Change;
        public float MaxHealth;
    }
    
    public interface IHealthEventHandler
    {
        void RegisterOnHealthChanged(Action<HealthChangedData> callback);
        void UnregisterOnHealthChanged(Action<HealthChangedData> callback);
        void RegisterOnAllHealthChanged(Action<GameObject, HealthChangedData> callback);
        void UnregisterOnAllHealthChanged(Action<GameObject, HealthChangedData> callback);
    }
}