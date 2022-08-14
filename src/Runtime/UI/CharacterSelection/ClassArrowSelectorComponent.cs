using MyRpg.Core.Models;

namespace MyRpg.UI.CharacterSelection
{
    public class ClassArrowSelectorComponent : AbstractArrowSelectorComponent
    {
        public Class GetSelectedClass()
        {
            return CurrentSelection switch
            {
                "mage" => Class.Mage,
                "warrior" => Class.Warrior,
                "priest" => Class.Priest,
                _ => Class.None
            };
        }
    }
}