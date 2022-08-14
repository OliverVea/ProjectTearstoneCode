using MyRpg.Core.Models;
using UnityEngine;

namespace MyRpg.Core
{
    public interface IPlayerJoinedSubscriber
    {
        void OnPlayerCharacterAdded(GameObject player);
        void OnPlayerCharacterRemoved(GameObject player);
    }
}