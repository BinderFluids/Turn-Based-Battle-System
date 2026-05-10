using System;
using System.Collections.Generic;
using System.Linq;
using Core.Enums;
using UnityEngine;
using Battle.Events.Windows;
using Codice.CM.Common;
using EventBus;

namespace Battle.Window
{
    /// <summary>
    /// Parameters for an action command (timed window, expected input band).
    /// </summary>
    public class ActionCommandWindow : Window
    {
        //Action command
        public int Duration => gradient.Frames; 
        private readonly ActionCommandTierGradient gradient;
        public ActionCommandTierGradient Gradient => gradient;
        
        //Outcome strategy
        private readonly IOutcomeStrategy outcomeStrategy;
        
        //Hold Data
        private readonly Dictionary<PlayerId, PlayerHoldData> holdData = new Dictionary<PlayerId, PlayerHoldData>();
        
        internal ActionCommandWindow(string id, List<PlayerId> expectedPlayerInputs, ActionCommandTierGradient gradient, IOutcomeStrategy outcomeStrategy)
        : base(id, expectedPlayerInputs)
        {
            this.gradient = gradient;
            this.outcomeStrategy = outcomeStrategy;
        }

        protected override void OnOpen()
        {
            //Broadcast ActionCommandWindow has been opened
            EventBus<ActionCommandWindowOpened>.Raise(
                new ActionCommandWindowOpened(Id, Duration)
            );
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
            => outcomeStrategy.Evaluate(this);
    }

    /// <summary>
    /// Builder for ActionCommandWindow. Default Outcome Strategy is ActionCommandOutcomeStrategy.
    /// </summary>
    public class ActionCommandWindowBuilder
    {
        private IOutcomeStrategy outcomeStrategy = new ActionCommandOutcomeStrategy(); 
        private readonly List<PlayerId> expectedInputs = new();
        
        public ActionCommandWindowBuilder WithOutcomeStrategy(IOutcomeStrategy strategy)
        {
            outcomeStrategy = strategy;
            return this; 
        }

        public ActionCommandWindowBuilder WithPlayerInput(PlayerId playerId)
        {
            expectedInputs.Add(playerId);
            return this; 
        }

        public ActionCommandWindow Build(string id, ActionCommandTierGradient gradient) 
            => new(id, expectedInputs, gradient, outcomeStrategy);
    }
}
