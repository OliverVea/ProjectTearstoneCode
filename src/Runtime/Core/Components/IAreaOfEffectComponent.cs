using MyRpg.Core.Models;

namespace MyRpg.Core.Components
{
    public interface IAreaOfEffectComponent
    {
        void ServerApplyEffect(SpellCasting spellCasting);
    }
}