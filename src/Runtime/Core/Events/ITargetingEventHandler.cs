using System;
using UnityEngine;

namespace MyRpg.Core.Events
{
    public interface ITargetingEventHandler
    {
        void RegisterOnNewTarget(Action<GameObject, GameObject> callback);
        void UnregisterOnNewTarget(Action<GameObject, GameObject> callback);
        
        void RegisterOnNewHoverTarget(Action<GameObject, GameObject> callback);
        void UnregisterOnNewHoverTarget(Action<GameObject, GameObject> callback);
    }
}