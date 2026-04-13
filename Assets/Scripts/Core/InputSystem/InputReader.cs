using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using static InputSystem; 


[CreateAssetMenu(menuName = "Create InputReader", fileName = "InputReader", order = 0)]
public class InputReader : ScriptableObject, IPlayerActions, IDialogueActions, IDebugActions
{
    [SerializeField] private InputActionType activeActionType; 
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
    
    private InputSystem inputActions;
    
    public void EnableInput(InputActionType inputActionType)
    {
        activeActionType = inputActionType;
        
        if (inputActionType != InputActionType.None)
        {
            if (inputActions == null)
            {
                OnFirstEnable();
            }
            inputActions.Enable();
        }

        switch (inputActionType)
        {
            case InputActionType.Player:
                inputActions.Player.Enable();
                inputActions.Dialogue.Disable();
                break;
            case InputActionType.Dialogue:
                inputActions.Dialogue.Enable();
                inputActions.Player.Disable();
                break;
            default:
                inputActions.Disable();
                break;
        }
        
        Debug.Log($"Player Enabled: {inputActions.Player.enabled} Dialogue Enabled: {inputActions.Dialogue.enabled}");
    }
    void OnFirstEnable()
    {
        inputActions = new InputSystem();
        inputActions.Player.SetCallbacks(this); 
        inputActions.Dialogue.SetCallbacks(this);
        
        inputActions.Debug.Enable();
        inputActions.Debug.SetCallbacks(this); 
        
        PlayerMove = new Vector2InputData(inputActions.Player.Move);
        Interact = new BoolInputData(inputActions.Player.Interact);
        MouseDelta = new Vector2InputData(inputActions.Player.Mouse); 
        
        DialogueMove = new Vector2InputData(inputActions.Dialogue.Move);
        Progress = new BoolInputData(inputActions.Dialogue.Progress);
        Skip = new BoolInputData(inputActions.Dialogue.Skip);
        Click = new BoolInputData(inputActions.Player.Click);
        
        DebugA = new BoolInputData(inputActions.Debug.A);
        PlayMicrogame = new BoolInputData(inputActions.Debug.PlayMicrogame);
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
        if (activeActionType == InputActionType.Player)
            PlayerMove.Trigger(context);
        else if (activeActionType == InputActionType.Dialogue)
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