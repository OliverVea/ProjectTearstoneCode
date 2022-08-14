using System;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

namespace MyRpg.Core.Events
{
    public class EventContent<TData>
    {
        public GameObject Invoker;
        public TData Data;
    }
    
    public class GenericEventHandler : NetworkBehaviour
    {
        private class EventContent
        {
            public GameObject Invoker;
            public object Data;
        }

        private static readonly Dictionary<Type, Dictionary<string, Action<EventContent>>> Events = new Dictionary<Type, Dictionary<string, Action<EventContent>>>();

        public static void Register<TEventData>(Action<EventContent<TEventData>> callback, string callbackId)
        {
            EnsureKeyExists(typeof(TEventData));
            Events[typeof(TEventData)][callbackId] = GetCallback(callback);
        }
        
        public static void Unegister<TEventData>(string callbackId)
        {
            EnsureKeyExists(typeof(TEventData));
            var callbacks = Events[typeof(TEventData)];
            if (callbacks.ContainsKey(callbackId)) callbacks.Remove(callbackId);
        }

        public static void Invoke<TEventData>(GameObject invoker, TEventData data)
        {
            EnsureKeyExists(typeof(TEventData));
            
            var eventContent = new EventContent
            {
                Invoker = invoker,
                Data = data
            };

            var callbacks = Events[typeof(TEventData)].Values;
            foreach (var callback in callbacks) callback.Invoke(eventContent);
        }

        private static void EnsureKeyExists(Type type)
        {
            if (Events.ContainsKey(type)) return;
            Events[type] = new Dictionary<string, Action<EventContent>>();
        }

        private static Action<EventContent> GetCallback<TEventData>(Action<EventContent<TEventData>> callback)
        {
            return content => callback(new EventContent<TEventData>
            {
                Invoker = content.Invoker,
                Data = (TEventData)content.Data
            });
        }
    }
}