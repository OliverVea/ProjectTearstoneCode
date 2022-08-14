using System;

namespace MyRpg.Core.Events
{
    public interface IEffectsEventHandler
    {
        void RegisterOnEffectApplied(Action<string> callback);
        void UnregisterOnEffectApplied(Action<string> callback);
        //void RegisterAllOnEffectApplied(Action<IEffectsComponent> callback);
        //void UnregisterAllOnEffectApplied(Action<IEffectsComponent> callback);
        
        void RegisterOnEffectRemoved(Action<string> callback);
        void UnregisterOnEffectRemoved(Action<string> callback);
        //void RegisterAllOnEffectRemoved(Action<IEffectsComponent> callback);
        //void UnregisterAllOnEffectRemoved(Action<IEffectsComponent> callback);
        
        void RegisterOnEffectExpired(Action<string> callback);
        void UnregisterOnEffectExpired(Action<string> callback);
        //void RegisterAllOnEffectExpired(Action<IEffectsComponent> callback);
        //void UnregisterAllOnEffectExpired(Action<IEffectsComponent> callback);
    }
}