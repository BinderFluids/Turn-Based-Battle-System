using System.Collections.Generic;
using Battle.BattleWindow.Enums;
using Battle.BattleWindow.OutcomeStrategy;
using JetBrains.Annotations;
using UnityEngine;

namespace Battle.BattleWindow
{
    /// <summary>
    /// Parameters for an action command (timed window, expected input band).
    /// </summary>
    public class ActionCommandWindow : Window
    {
        public int Duration { get; }
        private readonly Dictionary<PlayerId, PlayerHoldData> holdData = new Dictionary<PlayerId, PlayerHoldData>();
        private readonly IOutcomeStrategy outcomeStrategy;

        public ActionCommandWindow(string id, int duration, List<PlayerId> expectedPlayerInputs, IOutcomeStrategy outcomeStrategy)
            : base(id, expectedPlayerInputs)
        {
            Duration = duration;
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
            {
                return this.outcomeStrategy.Evaluate(this);
            }
            
            ActionCommandOutcome outcome = new DefaultOutcomeStrategy().Evaluate(this);
            return outcome;
        }
    }
}
