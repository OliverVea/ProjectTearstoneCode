using MyRpg.Core.Models;

namespace MyRpg.Core.Modifiers
{
    public interface ICharacterStatusProvider
    {
        CharacterStatus GetCharacterStatus();
    }
}