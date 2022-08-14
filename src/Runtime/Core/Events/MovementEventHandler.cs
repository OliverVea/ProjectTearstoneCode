using System;
using UnityEngine;

namespace MyRpg.Core.Events
{
    public class MovementEventHandler
    {
        private static Action<GameObject, Vector2> _onMovement;

        public static void RegisterOnMovement(Action<GameObject, Vector2> callback)
            => _onMovement += callback;

        public static void UnregisterOnMovement(Action<GameObject, Vector2> callback)
            => _onMovement -= callback;

        public static void InvokeOnMovement(GameObject source, Vector2 movement)
            => _onMovement?.Invoke(source, movement);
    }
}