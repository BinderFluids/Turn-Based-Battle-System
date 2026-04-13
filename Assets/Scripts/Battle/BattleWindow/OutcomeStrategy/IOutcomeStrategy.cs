using System.Collections.Generic;
using Battle.BattleWindow.Enums;

namespace Battle.BattleWindow.OutcomeStrategy
{
    public interface IOutcomeStrategy
    {
        public ActionCommandOutcome Evaluate(
            ActionCommandWindow window
        );
    }
}