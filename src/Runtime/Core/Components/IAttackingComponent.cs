namespace MyRpg.Core.Components
{
    public interface IAttackingComponent : IBase
    {
        bool IsAttacking { get; set; }
        bool IsInRange();

        void OnHit();
    }
}