using Battle.Events.Windows;
using Core.Enums;
using Cysharp.Threading.Tasks;
using EventBus;
using UnityEngine;
using UnityEngine.Events;
using UnityUtils;

namespace Battle.Window
{
    public class BattleWindowService : Singleton<BattleWindowService>
    {
        private Window currentWindow;
        
        [SerializeField]
        private BattleInputReader battleInputReader;
        
        private UnityAction<bool> _playerOneHandler;
        private UnityAction<bool> _playerTwoHandler;
        
        public ActionCommandWindowBuilder ActionCommandWindowBuilder { get; private set; }

        private void Start()
        {
            battleInputReader ??= BattleUtils.InputReader;
            ActionCommandWindowBuilder = new ActionCommandWindowBuilder();
            
            SubscribeToEvents();
        }

        /// <summary>
        /// Subscribe to input events so we can pass them onto who ever subscribes to the windowId.
        /// </summary>
        void SubscribeToEvents()
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

            //Step through each frame of the window's duration and exit early on player input
            for (int i = 0; i < actionCommandWindow.Duration; i++)
            {
                //Tanner - TODO: I don't love this. 
                if (actionCommandWindow.TryGetHoldData(PlayerId.PlayerOne, out var playerOneData) &&
                    playerOneData.HasPressed)
                    break;
                if (actionCommandWindow.TryGetHoldData(PlayerId.PlayerTwo, out var playerTwoData) &&
                    playerTwoData.HasPressed)
                    break;

                await UniTask.DelayFrame(1); 
            }
            
            //Derek - TODO: BAD CODE, MAKE BETTER
            Reset();
            return actionCommandWindow.DetermineOutcome();
        }
        
        /// <summary>
        /// Resets all the necessary variables. This should be called on Battle end or Action end.
        /// </summary>
        public void Reset()
        {
            currentWindow?.Close();
            currentWindow = null;
        }

        private void OnDestroy()
        {
            Reset();
            battleInputReader.PlayerOne.Action -= _playerOneHandler;
            battleInputReader.PlayerTwo.Action -= _playerTwoHandler;
        }
    }
}