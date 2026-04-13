using System.Collections.Generic;

namespace Battle.Input
{
    /// <summary>
    /// This defines the parameters for the ActionCommand
    ///
    /// These will only have on button that you probably will press? Not sure yet...
    /// </summary>
    public struct ActionCommandSpec
    {
        public string id;
        public float Duration;
        
        // Who do we expect ActionCommandsFrom
        // Potentially we will have this extend from PlayerBattleEntity once we have that
        public List<BattleEntity> actors;
        
        // This is when we will expect the ActionCommand Input
        // has to be between the Duration times
        public float WindowOpenTime;
        public float WindowCloseTime;
    }
}