using Core.Enums;
using UnityEngine;
using UnityEngine.InputSystem;


namespace Battle
{
    [CreateAssetMenu(menuName = "Battle/Utility Data", fileName = "BattleUtilityData", order = 0)]
    public class PlayerInputData : ScriptableObject
    {
        [SerializeField] private InputActionReference PlayerOne; 
        [SerializeField] private InputActionReference PlayerTwo;

        public InputAction GetInputActionByPlayerID(PlayerId id)
        {
            return id switch
            {
                PlayerId.PlayerOne => PlayerOne.ToInputAction(),
                PlayerId.PlayerTwo => PlayerTwo.ToInputAction(),
                _ => null
            };
        }
    }
}