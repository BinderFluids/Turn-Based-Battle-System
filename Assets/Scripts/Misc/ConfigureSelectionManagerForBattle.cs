using System;
using EventBus;
using UnityEngine;

namespace Misc
{
    public class ConfigureSelectionManagerForBattle : MonoBehaviour
    {
        [SerializeField] private BattleInputReader inputReader;
        
        private void Start()
        {
            if (inputReader.TryGetCurrentPlayerInput(out var playerInput))
                SelectionManager.Instance.Configure(playerInput, inputReader.Move); 
        }
    }
}