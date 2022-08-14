using UnityEngine;

namespace MyRpg.Core.Constants
{
    public static class Colors
    {
        # region Social

        public static readonly Color Party = new Color(0.67f, 0.67f, 1f);
        public static readonly Color PartyLeader = new Color(0.47f, 0.78f, 1f);

        #endregion
        
        # region Faction
        
        public static readonly Color Friendly = Color.green;
        public static readonly Color Enemy = Color.red;
        public static readonly Color Neutral = Color.yellow;
        
        #endregion
        
        # region UnitFrames
        
        public static readonly Color HealthHigh = Color.green;
        public static readonly Color HealthMedium = Color.yellow;
        public static readonly Color HealthLow = Color.red;
        
        # endregion
        
        # region System

        public static readonly Color Error = Color.magenta;

        # endregion
    }
}