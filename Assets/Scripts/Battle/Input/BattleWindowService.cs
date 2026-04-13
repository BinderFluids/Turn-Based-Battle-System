using System;
using Cysharp.Threading.Tasks;
using EventBus;
using UnityEngine;
using UnityUtils;

namespace Battle.Input
{
    public class BattleWindowService : Singleton<BattleWindowService>
    {
        private Window currentWindow;
        private EventBinding<WindowInputEvent> windowInputEventBinding;

        private void Start()
        {
            this.SubscribeToEvents();
        }

        /// <summary>
        /// Subscribe to input events so we can pass them onto who ever subscribes to the windowId.
        /// </summary>
        public void SubscribeToEvents()
        {
            windowInputEventBinding = new EventBinding<WindowInputEvent>(HandleWindowEvent);
            EventBus<WindowInputEvent>.Register(windowInputEventBinding);
        }

        private void HandleWindowEvent(WindowInputEvent e)
        {
            if (currentWindow == null) return;
            currentWindow.HandleInput(e);
        }

        /// <summary>Initiates a single ActionCommand and returns the outcome after specified duration</summary>
        /// <returns></returns>
        public async UniTask<ActionCommandOutcome> RunActionCommandAsync(ActionCommandWindow actionCommandWindow)
        {
            currentWindow = actionCommandWindow;
            currentWindow.Open();
            EventBus<ActionCommandWindowOpened>.Raise(new ActionCommandWindowOpened(actionCommandWindow.Id, actionCommandWindow.Duration, actionCommandWindow.Threshold));
            await UniTask.WaitForSeconds(actionCommandWindow.Duration); 
            return actionCommandWindow.DetermineOutcome();
        }
        
        /// <summary>
        /// Resets all the necessary variables. This should be called on Battle end or Action end.
        /// </summary>
        public void Reset()
        {
            this.currentWindow = null;
        }

        private void OnDestroy()
        {
            this.currentWindow = null;
            EventBus<WindowInputEvent>.Deregister(windowInputEventBinding);
        }
    }
}