namespace MyRpg.Core.Helpers
{
    public class BoolHelper
    {
        private bool _previousState;

        public bool NowTrue { get; private set; }
        public bool NowFalse { get; private set; }

        public void Update(bool newState)
        {
            NowTrue = !_previousState && newState;
            NowFalse = _previousState && !newState;

            _previousState = newState;
        }
    }
}