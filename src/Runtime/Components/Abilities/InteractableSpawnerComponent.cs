using System.Collections.Generic;
using Mirror;
using MyRpg.Core.Components;
using MyRpg.Runtime.Control;
using UnityEngine;

namespace MyRpg.Components.Abilities
{
    public class InteractableSpawnerComponent : AbstractSpawnerComponent, IInteractableComponent
    {
        [SerializeField] private List<Unit> units;

        [Server]
        public void ServerInteract()
        {
            foreach (var unit in units) SpawnUnit(unit);
        }
    }
}