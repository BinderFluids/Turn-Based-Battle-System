using System.Collections.Generic;

namespace Battle.Input
{
    /// <summary>
    /// Parameters for an action command (timed window, expected input band).
    /// </summary>
    public class ActionCommandWindow : Window
    {
        public float Duration;
        public float Threshold;

        private Dictionary<InputType, float> firstInputTimes = new Dictionary<InputType, float>();
        

        public ActionCommandWindow(string id, List<InputType> expectedInputs, float duration, float threshold)
            : base(id, expectedInputs)
        {
            Duration = duration;
            Threshold = threshold;
        }

        public override void HandleInput(WindowInputEvent evt)
        {
            if (!ExpectedInputs.Contains(evt.InputType)) return;
            if (firstInputTimes.ContainsKey(evt.InputType)) return;
            
            firstInputTimes[evt.InputType] = evt.TimeSeconds;
        }

        public ActionCommandOutcome DetermineOutcome()
        {
            // It automatically fails if we dont have firstInputTimes for all the expectedInputs or if there were no inputs
            if(firstInputTimes.Count == 0) return ActionCommandOutcome.Fail();
            if(firstInputTimes.Count != ExpectedInputs.Count) return ActionCommandOutcome.Fail();
            
            // Now we need to take into account timing and threshold
            
            
            ActionCommandOutcome outcome = ActionCommandOutcome.Succeed(ActionCommandTier.GOOD);
            return outcome;
        }
    }
}
