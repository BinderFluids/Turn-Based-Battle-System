using System;
using System.Collections.Generic;
using Battle.Enums;
using Core.Enums;

namespace Battle.Window
{
    public class ActionCommandOutcomeStrategy : IOutcomeStrategy
    {
        private ActionCommandTier EvaluateTier(int error, int duration)
        {
            float normalized = error / (float)duration;
            
            if (normalized <= 0.1f) return ActionCommandTier.EXCELLENT;
            if (normalized <= 0.25f) return ActionCommandTier.GREAT;
            if (normalized <= 0.5f) return ActionCommandTier.GOOD;
            return ActionCommandTier.OKAY;
        }
        
        public ActionCommandOutcome Evaluate(ActionCommandWindow window)
        {
            //int referenceFrame = window.StartFrame + window.Duration;
            ActionCommandTier worstTier = ActionCommandTier.EXCELLENT;
            bool hasAny = false;

            foreach (PlayerId player in window.ExpectedInputs)
            {
                if (!window.TryGetHoldData(player, out var data))
                    return ActionCommandOutcome.Fail();

                ActionCommandTier tier = window.Gradient.Evaluate(window.Duration);
                hasAny = true;

                // keep worst result
                if (tier < worstTier)
                    worstTier = tier;
            }

            if (!hasAny)
                return ActionCommandOutcome.Fail();

            return ActionCommandOutcome.Succeed(worstTier);
        }
    }
}