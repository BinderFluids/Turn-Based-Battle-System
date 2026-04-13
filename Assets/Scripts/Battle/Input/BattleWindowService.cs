using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityUtils;

namespace Battle.Input
{
    public class BattleWindowService : Singleton<BattleWindowService>
    {

        private WindowSpec currentWindow;

        /// <summary>
        /// Subscribe to input events so we can pass them onto who ever subscribes to the windowId.
        /// </summary>
        public void SubscribeToEvents()
        {
            // TODO: Need to figure out input events
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async UniTask<ActionCommandOutcome> RunActionCommandAsync(ActionCommandSpec actionCommandSpec)
        {
            // TODO: Initiate a listener for a command that will process events
            // TODO: Fire an event that the actionCommand window has opened
            // TODO: await for the duration of the actionCommand
            // TODO: determine if we got a good input during that time
            // TODO: return an outcome
            return new ActionCommandOutcome();
        }
        
        /// <summary>
        /// Resets all the necessary variables. This should be called on Battle end or Action end.
        /// </summary>
        public void Reset()
        {
            this.currentWindow = null;
        }
    }
}