using System;
using Battle.BattleEntity;
using Battle.BattleWindow.Enums;
using Cysharp.Threading.Tasks;
using EventBus;
using UnityEngine;
using UnityEngine.Events;
using UnityUtils;

namespace Battle.BattleWindow
{
    public class BattleWindowService : Singleton<BattleWindowService>
    {
        private Window currentWindow;
        
        [SerializeField]
        private BattleInputReader battleInputReader;
        
        private UnityAction<bool> _playerOneHandler;
        private UnityAction<bool> _playerTwoHandler;

        private void Start()
        {
            battleInputReader.EnableInput(InputActionType.Player);
            SubscribeToEvents();
        }

        /// <summary>
        /// Subscribe to input events so we can pass them onto who ever subscribes to the windowId.
        /// </summary>
        public void SubscribeToEvents()
        {
            _playerOneHandler = (pressed) => HandleInputEvent(PlayerId.PlayerOne, pressed);
            _playerTwoHandler = (pressed) => HandleInputEvent(PlayerId.PlayerTwo, pressed);

            battleInputReader.PlayerOne.Action += _playerOneHandler;
            battleInputReader.PlayerTwo.Action += _playerTwoHandler;
        }

        private void HandleInputEvent(PlayerId player, bool isPressed)
        {
            if (currentWindow == null) return;
            currentWindow.HandleInput(player, isPressed);
        }

        /// <summary>Initiates a single ActionCommand and returns the outcome after specified duration</summary>
        /// <returns></returns>
        public async UniTask<ActionCommandOutcome> RunActionCommandAsync(ActionCommandWindow actionCommandWindow)
        {
            currentWindow = actionCommandWindow;
            currentWindow.Open();
            EventBus<ActionCommandWindowOpened>.Raise(
                new ActionCommandWindowOpened(actionCommandWindow.Id, actionCommandWindow.Duration)
            );
            await UniTask.DelayFrame(actionCommandWindow.Duration); 
            
            // TODO: BAD CODE, MAKE BETTER
            currentWindow.Close();
            currentWindow = null;
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
            this.Reset();
            battleInputReader.PlayerOne.Action -= _playerOneHandler;
            battleInputReader.PlayerTwo.Action -= _playerTwoHandler;
        }
    }
}