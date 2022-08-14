using System;

namespace MyRpg.Core.Events
{
    public class ManaChangedData
    {
        public float NewMana;
        public float Change;
        public float MaxMana;
    }
    
    public interface IManaEventHandler
    {
        void RegisterOnManaChanged(Action<ManaChangedData> callback);
        void UnregisterOnManaChanged(Action<ManaChangedData> callback);
        
    }
}