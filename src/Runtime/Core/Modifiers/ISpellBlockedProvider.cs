namespace MyRpg.Core.Modifiers
{
    public interface ISpellBlockedProvider
    {
        bool IsSpellBlocked(string spellId);
    }
}