using System;
using System.Collections.Generic;
using System.Linq;
using Mirror;
using MyRpg.Core.Components;
using UnityEngine;

namespace MyRpg.Components
{
    public class PatrolPathComponent : NetworkBehaviour, IPatrolPathComponent
    {
        private enum PathType { OneWay, Loop }

        [SerializeField] private PathType _pathType;
        [SerializeField] private float maxSpeed = 2f;
        [SerializeField] private float nodeSize = 0.2f;
        [SerializeField] private float[] nodePauses;
        private Vector2[] _waypoints;
        
        private void OnDrawGizmos()
        {
            var children = Enumerable.Range(0, transform.childCount).Select(transform.GetChild).ToList();

            if (!children.Any()) return;
            
            Gizmos.color = Color.white;
            foreach (var child in children)
                Gizmos.DrawSphere(child.position, nodeSize);
            
            if (children.Count() < 2) return;

            var linePoints = children.ToList();
            if (_pathType == PathType.Loop) linePoints.Add(children.First());

            Gizmos.color = Color.white * 0.85f;
            for (int i = 0; i < linePoints.Count - 1; i++)
                Gizmos.DrawLine(linePoints[i].position, linePoints[i + 1].position);
        }

        private void Awake()
        {
            var waypoints = new List<Vector2>();
            
            for (int i = 0; i < transform.childCount; i++)
            {
                waypoints.Add(transform.GetChild(i).position);
            }

            _waypoints = waypoints.ToArray();
        }

        float IPatrolPathComponent.GetPause(int targetPatrolWaypointIndex)
        {
            return GetPause(targetPatrolWaypointIndex);
        }

        public int GetNextIndex(int index)
        {
            if (_pathType == PathType.OneWay) return Mathf.Min(index + 1, _waypoints.Length - 1);
            if (_pathType == PathType.Loop) return (index + 1) % _waypoints.Length;
            return 0;
        }

        public Vector2 GetWaypoint(int waypointIndex)
        {
            if (waypointIndex < _waypoints.Length) return _waypoints[waypointIndex];
            return Vector2.zero;
        }

        public float GetMaxSpeed()
        {
            return maxSpeed;
        }

        public float GetPause(int index) => nodePauses.Length > index ? nodePauses[index] : 0;
    }
}