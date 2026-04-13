using UnityEngine;

namespace Battle.BattleWindow.OutcomeStrategy
{
    public abstract class ScriptableOutcomeStrategy : ScriptableObject, IOutcomeStrategy
    {
        public abstract ActionCommandOutcome Evaluate(ActionCommandWindow window);
    }
}