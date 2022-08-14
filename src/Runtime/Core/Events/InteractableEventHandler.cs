using System;
using UnityEngine;

namespace MyRpg.Core.Events
{
    public static class InteractableEventHandler
    {
        private static Action<GameObject, int> _onInteractablesChanged =
            (g, i) => Debug.Log($"Player {g} now has {i} interactables.");

        public static void RegisterOnInteractablesChanged(Action<GameObject, int> callback)
            => _onInteractablesChanged += callback;

        public static void UnregisterOnInteractablesChanged(Action<GameObject, int> callback)
            => _onInteractablesChanged -= callback;

        public static void InvokeOnInteractablesChanged(GameObject source, int interactablesCount)
            => _onInteractablesChanged?.Invoke(source, interactablesCount);
    }
}