using System;
using UnityEngine;

namespace MyRpg.Core.Events
{
    public class PlayerEventHandler
    {
        private static Action<GameObject[]> _onPartyChanged;

        public static void RegisterOnMovement(Action<GameObject[]> callback)
            => _onPartyChanged += callback;

        public static void UnregisterOnMovement(Action<GameObject[]> callback)
            => _onPartyChanged -= callback;

        public static void InvokeOnMovement(GameObject[] party)
            => _onPartyChanged?.Invoke(party);
    }
}