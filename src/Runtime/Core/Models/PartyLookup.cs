using System;
using System.Linq;
using Mirror;
using MyRpg.Core.Events;
using MyRpg.Core.Events.EventData;
using MyRpg.Core.Lookup;
using UnityEngine;

namespace MyRpg.Core.Models
{
    public class PartyLookup : NetworkBehaviour
    {
        private string OnPlayerJoinedId => nameof(PartyLookup) + ToString() + nameof(OnPlayerJoined);
        private readonly SyncList<string> _characterIds = new SyncList<string>();

        private int _lastPartySize;
        private static PartyLookup _instance;

        private void Awake()
        {
            _instance = this;
        }

        private void Update()
        {
            if (_characterIds.Count == _lastPartySize) return;
            _lastPartySize = _characterIds.Count;
            
            GenericEventHandler.Invoke(gameObject, new PartyChangedEvent { PartyMembers = GetParty() });
            Debug.Log($"Now {_characterIds.Count} players.");
        }
        
        private void OnEnable()
        {
            GenericEventHandler.Register<PlayerJoinedEvent>(OnPlayerJoined, OnPlayerJoinedId);
        }

        private void OnDisable()
        {
            GenericEventHandler.Unegister<PlayerJoinedEvent>(OnPlayerJoinedId);
        }

        [Server]
        private void OnPlayerJoined(EventContent<PlayerJoinedEvent> eventContent)
        {
            if (_characterIds.Contains(eventContent.Data.CharacterId)) return;
            _characterIds.Add(eventContent.Data.CharacterId);
        }

        public static GameObject[] GetParty()
        {
            return _instance?._characterIds
                .Select(GameObjectLookup.GetWithCharacterId).ToArray() ?? Array.Empty<GameObject>(); 
        }
    }
}