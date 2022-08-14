using System.Collections.Generic;
using MyRpg.Core.Models;
using UnityEngine;

namespace MyRpg.Core
{
    public interface IPlayerController: IBase
    {
        KeyBinding[] GetSpellBindings();
        void RegisterInteractables(IEnumerable<GameObject> gameObject);
        void UnregisterInteractables(IEnumerable<GameObject> gameObject);
        IEnumerable<GameObject> GetNearbyEnemies();
    }
}