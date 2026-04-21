namespace Battle.BattleWindow.OutcomeStrategy
{
    public interface IOutcomeStrategy
    {
        public ActionCommandOutcome Evaluate(
            ActionCommandWindow window
        );
    }
}