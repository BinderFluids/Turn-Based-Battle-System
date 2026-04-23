namespace Battle
{
    public interface IOutcomeStrategy
    {
        public ActionCommandOutcome Evaluate(
            ActionCommandWindow window
        );
    }
}