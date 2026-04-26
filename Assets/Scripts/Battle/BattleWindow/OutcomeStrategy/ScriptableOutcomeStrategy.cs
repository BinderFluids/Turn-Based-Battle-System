using UnityEngine;

namespace Battle.Window
{
    public abstract class ScriptableOutcomeStrategy : ScriptableObject, IOutcomeStrategy
    {
        public abstract ActionCommandOutcome Evaluate(ActionCommandWindow window);
    }
}