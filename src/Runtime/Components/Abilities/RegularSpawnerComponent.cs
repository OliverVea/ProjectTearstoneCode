using System;
using Mirror;
using MyRpg.Runtime.Control;
using UnityEngine;

namespace MyRpg.Components.Abilities
{
    public class RegularSpawnerComponent : AbstractSpawnerComponent
    {
        [Serializable]
        protected class Group
        {

            [SerializeField] public float offset;
            [SerializeField] public Unit[] units;

            public bool spawned;
        }

        [SerializeField] private Group[] _spawnGroups;
        [SerializeField] private float _loopTime = float.MaxValue;

        [SerializeField] private float _currentTime;

        [ServerCallback]
        private void Update()
        {
            _currentTime += Time.deltaTime;
            CheckGroups();
            if (_currentTime > _loopTime) ResetLoop();
        }

        private void CheckGroups()
        {
            foreach (var group in _spawnGroups)
            {
                if (!group.spawned && _currentTime >= group.offset) SpawnGroup(group);
            }
        }

        private void SpawnGroup(Group group)
        {
            group.spawned = true;
            foreach (var unit in group.units) SpawnUnit(unit);
        }

        private void ResetLoop()
        {
            _currentTime = 0;
            foreach (var group in _spawnGroups) group.spawned = false;
        }
    }
}