using System;
using Mirror;
using MyRpg.Core;
using MyRpg.Core.Components;
using UnityEngine;

namespace MyRpg.Runtime.Control
{
    [Serializable]
    public abstract class AbstractSpawnerComponent : NetworkBehaviour
    {
        [Serializable]
        protected  class Unit
        {
            [SerializeField] public GameObject unit;
            [SerializeField] public GameObject patrol;
        }

        protected void SpawnUnit(Unit unit)
        {
            var spawnPosition = unit.patrol.transform.GetChild(0).position;
            var unitInstance = Instantiate(unit.unit, spawnPosition, Quaternion.identity, transform);
            var patrolPathComponent = unit.patrol.GetComponent<IPatrolPathComponent>();
            unitInstance.GetComponent<IAIController>().SetPatrolPath(patrolPathComponent, 0);
            NetworkServer.Spawn(unitInstance);
        }
    }
}