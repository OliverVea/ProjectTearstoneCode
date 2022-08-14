using UnityEngine;

namespace MyRpg.Core.Components
{
    public interface IPatrolPathComponent
    {
        Vector2 GetWaypoint(int waypointIndex);
        float GetMaxSpeed();
        float GetPause(int targetPatrolWaypointIndex);
        int GetNextIndex(int targetPatrolWaypointIndex);
    }
}