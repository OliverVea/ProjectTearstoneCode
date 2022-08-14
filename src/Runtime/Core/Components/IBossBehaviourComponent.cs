namespace MyRpg.Core.Components
{
    public interface IBossBehaviourComponent
    {
        void ServerExecuteBossBehaviour();
        bool DoNormalBehaviour();
    }
}