using MyRpg.Core.Components;

namespace MyRpg.Core
{
    public interface IAIController
    {
        void SetPatrolPath(IPatrolPathComponent patrolPathComponent, int startingWaypointIndex);
    }
}