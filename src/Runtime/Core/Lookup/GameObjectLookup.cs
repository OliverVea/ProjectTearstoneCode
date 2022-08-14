using System.Collections.Generic;
using System.Linq;
using MyRpg.Core.Components;
using UnityEngine;

namespace MyRpg.Core.Lookup
{
    public class GameObjectLookup
    {
        private static Dictionary<string, GameObject> _characterIdLookup = new Dictionary<string, GameObject>();
        
        public static GameObject GetWithCharacterId(string characterId)
        {
            if (_characterIdLookup.ContainsKey(characterId) && _characterIdLookup[characterId] != null) 
                return _characterIdLookup[characterId];
            
            var gameObjects = GameObject.FindGameObjectsWithTag("Character");
            _characterIdLookup = gameObjects
                .Select(x => new { StatusComponent = x.GetComponent<IStatusComponent>(), GameObject = x })
                .Where(x => x.StatusComponent.GetCharacterId() != "")
                .ToDictionary(x => x.StatusComponent.GetCharacterId(), x => x.GameObject);

            return _characterIdLookup[characterId];
        }
    }
}