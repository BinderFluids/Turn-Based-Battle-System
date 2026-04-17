using UnityEngine;
using UnityEngine.InputSystem;

[CreateAssetMenu(menuName = "Input Reader/Battle Input", fileName = "BattleInputReader", order = 0)]
public class BattleInputReader : InputReader<BattleInput>, BattleInput.IPlayerActions
{
    public BoolInputData PlayerOne;
    public BoolInputData PlayerTwo;
    private PlayerId currentPlayerInput;

    public BoolInputData Confirm => PlayerOne;
    public Vector2InputData Move;
    
    protected override void OnEnableInput(InputActionType inputActionType)
    {
        if (inputActionType == InputActionType.Player) InputActions.Player.Enable();
    }

    protected override void OnInputActionsCreated()
    {
        InputActions.Player.SetCallbacks(this);
        
        PlayerOne = new BoolInputData(InputActions.Player.PlayerOne);
        PlayerTwo = new BoolInputData(InputActions.Player.PlayerTwo); 
        Move = new Vector2InputData(InputActions.Player.Move);
        
        SetCurrentPlayer(PlayerId.None);
    }

    public void SetCurrentPlayer(PlayerId input) => currentPlayerInput = input;
    public bool TryGetCurrentPlayerInput(out BoolInputData playerBoolInputData)
    {
        playerBoolInputData = currentPlayerInput switch
        {
            PlayerId.PlayerOne => PlayerOne,
            PlayerId.PlayerTwo => PlayerTwo,
            _ => null
        };
        
        return playerBoolInputData != null;
    }

    public void OnPlayerOne(InputAction.CallbackContext context)
    {
        PlayerOne.Trigger(context);
    }

    public void OnPlayerTwo(InputAction.CallbackContext context) => PlayerTwo.Trigger(context);
    public void OnMove(InputAction.CallbackContext context) => Move.Trigger(context);
    
}