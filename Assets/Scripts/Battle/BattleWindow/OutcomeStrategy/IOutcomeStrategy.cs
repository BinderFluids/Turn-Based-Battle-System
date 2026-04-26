namespace Battle.Window
{
    public interface IOutcomeStrategy
    {
        public ActionCommandOutcome Evaluate(
            ActionCommandWindow window
        );
    }
}