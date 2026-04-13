namespace Battle.Input
{
    public readonly struct ActionCommandOutcome
    {
        public bool Success { get; }
        public ActionCommandTier? Tier { get; }

        public ActionCommandOutcome(bool success, ActionCommandTier? tier)
        {
            Success = success;
            Tier = tier;
        }

        public static ActionCommandOutcome Fail()
        {
            return new ActionCommandOutcome(false, null);
        }

        public static ActionCommandOutcome Succeed(ActionCommandTier tier)
        {
            return new ActionCommandOutcome(true, tier);
        }
    }
}