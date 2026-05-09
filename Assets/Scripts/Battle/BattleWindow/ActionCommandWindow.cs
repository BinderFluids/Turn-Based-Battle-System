using System.Collections.Generic;
using Core.Enums;
using UnityEngine;

namespace Battle.Window
{
    /// <summary>
    /// Parameters for an action command (timed window, expected input band).
    /// </summary>
    public class ActionCommandWindow : Window
    {
        private readonly Dictionary<PlayerId, PlayerHoldData> holdData = new Dictionary<PlayerId, PlayerHoldData>();
        private readonly IOutcomeStrategy outcomeStrategy;
        public int Duration => gradient.Frames; 
        private readonly ActionCommandTierGradient gradient;
        public ActionCommandTierGradient Gradient => gradient;
        private readonly ActionCommandOutcomeStrategy _actionCommandOutcomeStrategy = new ActionCommandOutcomeStrategy();
        
        public ActionCommandWindow(string id, List<PlayerId> expectedPlayerInputs, ActionCommandTierGradient gradient, IOutcomeStrategy outcomeStrategy)
        : base(id, expectedPlayerInputs)
        {
            this.gradient = gradient;
            this.outcomeStrategy = outcomeStrategy;
        }

        public bool TryGetHoldData(PlayerId playerId, out PlayerHoldData data)
        {
            return holdData.TryGetValue(playerId, out data);
        }

        public override void HandleInput(PlayerId playerId, bool isPressed)
        {
            if (!ExpectedInputs.Contains(playerId)) return;

            if (!holdData.TryGetValue(playerId, out var data))
            {
                data = new PlayerHoldData();
            }
                

            if (isPressed)
            {
                // only first press counts
                if (!data.HasPressed)
                {
                    data.PressFrame = Time.frameCount;
                }
            }
            else
            {
                // only first release counts
                if (data.HasPressed && !data.HasReleased)
                {
                    data.ReleaseFrame = Time.frameCount;
                }
            }

            holdData[playerId] = data;
        }

        public ActionCommandOutcome DetermineOutcome()
        {
            if (outcomeStrategy != null)
                return outcomeStrategy.Evaluate(this);
            
            
            ActionCommandOutcome outcome = _actionCommandOutcomeStrategy.Evaluate(this);
            return outcome;
        }
    }
}
