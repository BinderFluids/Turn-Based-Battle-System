using System.Collections.Generic;

namespace Battle.Input
{
    /// <summary>
    /// Parameters for an action command (timed window, expected input band).
    /// </summary>
    public class ActionCommandSpec : WindowSpec
    {
        public float Duration;

        // Who we expect action command input from (e.g. player entities later).
        public List<BattleEntity> actors;

        // Valid input must fall between these times (relative to window start).
        public float WindowOpenTime;
        public float WindowCloseTime;
    }
}
