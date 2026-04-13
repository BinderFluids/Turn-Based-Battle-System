namespace Battle.Input
{
    public readonly struct ActionCommandOutcome
    {
        public bool Success { get; }
        public ActionCommandTier? Tier { get; }
        public float? InputTime { get; }

        public ActionCommandOutcome(bool success, ActionCommandTier? tier, float? inputTime)
        {
            Success = success;
            Tier = tier;
            InputTime = inputTime;
        }

        public static ActionCommandOutcome Fail()
        {
            return new ActionCommandOutcome(false, null, null);
        }

        public static ActionCommandOutcome Succeed(ActionCommandTier tier, float inputTime)
        {
            return new ActionCommandOutcome(true, tier, inputTime);
        }
    }
}