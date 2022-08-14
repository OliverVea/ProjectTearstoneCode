using System;
using UnityEngine;
using UnityEngine.AI;

namespace MyRpg.Components
{
    public class NavMeshAgentStopRotatingComponent : MonoBehaviour
    {
        private NavMeshAgent _agent;

        private void Awake()
        {
            _agent = GetComponent<NavMeshAgent>();
            _agent.updateUpAxis = false;
        }
    }
}