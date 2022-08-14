using System;
using UnityEngine;
using MyRpg.Core.Components;

namespace MyRpg.Core.Models
{
    [Serializable]
    public class PatrolProgress
    {
        private readonly IPatrolPathComponent _patrolPathComponent;
        private int _waypointIndex;
            
        public float TimeWaited;
        public float MovementSpeed => _patrolPathComponent.GetMaxSpeed();

        public PatrolProgress(IPatrolPathComponent patrolPathComponent, int startingIndex)
        {
            _patrolPathComponent = patrolPathComponent;
            _waypointIndex = startingIndex;
        }

        public Vector2 Waypoint => _patrolPathComponent.GetWaypoint(_waypointIndex);
        public float ToWait => _patrolPathComponent.GetPause(_waypointIndex);
        public void NextWaypoint() => _waypointIndex = _patrolPathComponent.GetNextIndex(_waypointIndex);
    }
}