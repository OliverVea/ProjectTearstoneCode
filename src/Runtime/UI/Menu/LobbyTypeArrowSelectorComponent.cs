using MyRpg.Core.Events;

namespace MyRpg.UI.Menu
{
    public class LobbyTypeArrowSelectorComponent : AbstractArrowSelectorComponent
    {
        public override void Increment()
        {
            base.Increment();
            
            MainMenuHandler.InvokeSetLobbyType(CurrentSelection);
        }

        public override void Decrement()
        {
            base.Decrement();
            
            MainMenuHandler.InvokeSetLobbyType(CurrentSelection);
        }
    }
}