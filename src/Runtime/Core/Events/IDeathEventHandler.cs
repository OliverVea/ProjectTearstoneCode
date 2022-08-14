using System;
using MyRpg.Core.Components;

namespace MyRpg.Core.Events
{
    public interface IDeathEventHandler
    {
        void RegisterOnDeath(Action callback);
        void UnregisterOnDeath(Action callback);
        void RegisterAllOnDeath(Action<IDeathComponent> callback);
        void UnregisterAllOnDeath(Action<IDeathComponent> callback);
    }
}