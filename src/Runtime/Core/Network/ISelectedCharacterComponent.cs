using MyRpg.Core.Models;

namespace MyRpg.Core.Network
{
    public interface ISelectedCharacterComponent
    {
        void SelectCharacter(Character newCharacter);
        bool IsCharacterSelected();
        void UnselectCharacter();
        Character GetCharacter();
    }
}