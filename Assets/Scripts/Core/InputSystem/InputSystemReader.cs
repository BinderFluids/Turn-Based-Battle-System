using UnityEngine;
using UnityEngine.InputSystem;

[CreateAssetMenu(menuName = "Input Reader/Input System", fileName = "InputSystemReader", order = 0)]
public class InputSystemReader : InputReader<InputSystem>, InputSystem.IPlayerActions, InputSystem.IDialogueActions, InputSystem.IDebugActions
{
    public BoolInputData Interact;
    public BoolInputData Progress;
    public BoolInputData Skip;
    public BoolInputData Click;
    public BoolInputData PlayMicrogame;
    
    public Vector2InputData PlayerMove;
    public Vector2InputData DialogueMove;
    public Vector2InputData MouseDelta; 
    public Vector2 MouseWorldPosition => 
        Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
    
    public BoolInputData DebugA;
    
    protected override void OnEnableInput(InputActionType inputActionType)
    {
        switch (inputActionType)
        {
            case InputActionType.Player:
                InputActions.Player.Enable();
                InputActions.Dialogue.Disable();
                break;
            case InputActionType.Dialogue:
                InputActions.Dialogue.Enable();
                InputActions.Player.Disable();
                break;
            default:
                InputActions.Disable();
                break;
        }
        
        Debug.Log($"Player Enabled: {InputActions.Player.enabled} Dialogue Enabled: {InputActions.Dialogue.enabled}");
    }
    protected override void OnInputActionsCreated()
    {
        InputActions.Player.SetCallbacks(this); 
        InputActions.Dialogue.SetCallbacks(this);
        
        InputActions.Debug.Enable();
        InputActions.Debug.SetCallbacks(this); 
        
        PlayerMove = new Vector2InputData(InputActions.Player.Move);
        Interact = new BoolInputData(InputActions.Player.Interact);
        MouseDelta = new Vector2InputData(InputActions.Player.Mouse); 
        
        DialogueMove = new Vector2InputData(InputActions.Dialogue.Move);
        Progress = new BoolInputData(InputActions.Dialogue.Progress);
        Skip = new BoolInputData(InputActions.Dialogue.Skip);
        Click = new BoolInputData(InputActions.Player.Click);
        
        DebugA = new BoolInputData(InputActions.Debug.A);
        PlayMicrogame = new BoolInputData(InputActions.Debug.PlayMicrogame);
    }
    
    public void OnA(InputAction.CallbackContext context)
    {
        DebugA.Trigger(context);
    }

    public void OnPlayMicrogame(InputAction.CallbackContext context)
    {
        PlayMicrogame.Trigger(context);
    }

    public void OnSkip(InputAction.CallbackContext context)
    {
        Skip.Trigger(context);
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        if (ActiveActionType == InputActionType.Player)
            PlayerMove.Trigger(context);
        else if (ActiveActionType == InputActionType.Dialogue)
            DialogueMove.Trigger(context);
    }

    public void OnMouse(InputAction.CallbackContext context)
    {
        MouseDelta.Trigger(context);
    }

    public void OnProgress(InputAction.CallbackContext context)
    {
        Progress.Trigger(context);
    }
    public void OnInteract(InputAction.CallbackContext context)
    {
        Interact.Trigger(context);
    }

    public void OnClick(InputAction.CallbackContext context)
    {
        Click.Trigger(context); 
    }
}