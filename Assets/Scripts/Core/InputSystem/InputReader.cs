using System;
using UnityEngine;
using UnityEngine.InputSystem;

public interface IInputReader
{
    Vector2InputData Move { get; }
    BoolInputData Select { get; } 
    
    void EnableInput(InputActionType inputActionType);
}

public abstract class InputReader<T> : ScriptableObject where T : IInputActionCollection2
{
    protected T InputActions { get; private set; }
    protected InputActionType ActiveActionType;

    public void EnableInput(InputActionType inputActionType)
    {
        ActiveActionType = inputActionType;
        
        if (inputActionType != InputActionType.None)
        {
            if (InputActions == null)
            {
                InputActions = Activator.CreateInstance<T>();
                OnInputActionsCreated();
            }
            
            InputActions.Enable();
        }
        
        OnEnableInput(inputActionType);
    }
    protected abstract void OnEnableInput(InputActionType inputActionType);
    
    protected abstract void OnInputActionsCreated();
}