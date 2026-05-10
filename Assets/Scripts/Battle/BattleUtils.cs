using Battle.Enums;
using UnityEngine;

namespace Battle
{
    public static class BattleUtils
    {
        public static PlayerInputData PlayerInputData
        {
            get
            {
                playerInputData ??= Resources.Load<PlayerInputData>("PlayerInputData");
                return playerInputData;
            }
        }
        private static PlayerInputData playerInputData;

        public static BattleInputReader InputReader
        {
            get
            {
                battleInputReader ??= InputManager.Instance.InputReader as BattleInputReader;
                return battleInputReader;
            }
        }
        private static BattleInputReader battleInputReader;
        
        
        public static Color TierToColor(ActionCommandTier tier)
        {
            return tier switch
            {
                ActionCommandTier.MISS => Color.darkGray,
                ActionCommandTier.OKAY => Color.yellow,
                ActionCommandTier.GOOD => Color.orange,
                ActionCommandTier.GREAT => Color.red,
                ActionCommandTier.EXCELLENT => Color.blue,
                _ => Color.black
            };
        }
    }
}