using MyRpg.Core.Models;

namespace MyRpg.Core.Components
{
    public interface ICooldownComponent : IBase
    {
        void ServerAddToCooldown(string spellid);
        Cooldown GetCooldown(string spellId);
        bool IsOnCooldown(string spellId);
    }
}