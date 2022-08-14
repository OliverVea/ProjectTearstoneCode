namespace MyRpg.Core.Components
{
    public interface IManaComponent : IBase
    {
        public float GetCurrentMana();
        public float GetMaxMana();
        void ServerSpendMana(float manaCost);
    }
}