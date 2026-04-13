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

        public ActionCommandWindow(string id, InputType expectedInputs, float duration, float threshold)
            : base(id, expectedInputs)
        {
            Duration = duration;
            Threshold = threshold;
        }

        public override void HandleInput(WindowInputEvent evt)
        {
            throw new System.NotImplementedException();
        }

        public ActionCommandOutcome DetermineOutcome()
        {
            ActionCommandOutcome outcome = ActionCommandOutcome.Fail();
            return outcome;
        }
    }
}
