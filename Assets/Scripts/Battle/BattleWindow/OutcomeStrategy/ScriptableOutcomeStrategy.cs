using UnityEngine;

namespace Battle
{
    public abstract class ScriptableOutcomeStrategy : ScriptableObject, IOutcomeStrategy
    {
        public abstract ActionCommandOutcome Evaluate(ActionCommandWindow window);
    }
}