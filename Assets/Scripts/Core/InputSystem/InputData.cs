using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public abstract class InputData<T>
{
    private bool enabled;
    private bool doDebug; 
    public bool Enabled => enabled; 
    public void Enable()
    {
        enabled = true;
    }
    public void Disable()
    {
        enabled = false;
    }

    public InputData(InputAction inputAction)
    {
        InputAction = inputAction;
    }
    public InputAction InputAction;
    
    public void DoDebug(bool doDebug) => this.doDebug = doDebug;

    protected void DebugLog(object message)
    {
        if (doDebug)
            Debug.Log(message); 
    }
    
    public bool IsPressed => InputAction.IsPressed();
    public bool WasPressedThisFrame => InputAction.WasPressedThisFrame();
    public bool WasReleasedThisFrame => InputAction.WasReleasedThisFrame();
    
    public T Value { get; protected set; }
    
    public event UnityAction ActionNoArgs = delegate { };
    public event UnityAction<T> Action = delegate { };

    protected void Invoke(T input)
    {
        ActionNoArgs?.Invoke();
        Action?.Invoke(input);
    }

    public abstract void Trigger(InputAction.CallbackContext context);
}

public class BoolInputData : InputData<bool>
{
    public BoolInputData(InputAction inputAction) : base(inputAction) { }
    

    public override void Trigger(InputAction.CallbackContext context)
    {
        switch (context.phase)
        {
            case InputActionPhase.Started:
                DebugLog("Started Input");
                Value = true;
                Invoke(true);
                break;
            case InputActionPhase.Canceled:
                DebugLog("Canceled Input");
                Value = false;
                Invoke(false);
                break;
        }
    }
}
public class Vector2InputData : InputData<Vector2>
{
    public Vector2InputData(InputAction inputAction) : base(inputAction) { }
    
    public bool UpIsPressed => InputAction.ReadValue<Vector2>().y > 0;
    public bool UpWasPressedThisFrame => InputAction.ReadValue<Vector2>().y > 0 && InputAction.WasPressedThisFrame();
    public bool UpWasReleasedThisFrame => InputAction.ReadValue<Vector2>().y > 0 && InputAction.WasReleasedThisFrame();
    
    public bool DownIsPressed => InputAction.ReadValue<Vector2>().y < 0;
    public bool DownWasPressedThisFrame => InputAction.ReadValue<Vector2>().y < 0 && InputAction.WasPressedThisFrame();
    public bool DownWasReleasedThisFrame => InputAction.ReadValue<Vector2>().y < 0 && InputAction.WasReleasedThisFrame();
    
    public bool LeftIsPressed => InputAction.ReadValue<Vector2>().x < 0;
    public bool LeftWasPressedThisFrame => InputAction.ReadValue<Vector2>().x < 0 && InputAction.WasPressedThisFrame();
    public bool LeftWasReleasedThisFrame => InputAction.ReadValue<Vector2>().x < 0 && InputAction.WasReleasedThisFrame();
    
    public bool RightIsPressed => InputAction.ReadValue<Vector2>().x > 0;
    public bool RightWasPressedThisFrame => InputAction.ReadValue<Vector2>().x > 0 && InputAction.WasPressedThisFrame();
    public bool RightWasReleasedThisFrame => InputAction.ReadValue<Vector2>().x > 0 && InputAction.WasReleasedThisFrame();
    
    public override void Trigger(InputAction.CallbackContext context)
    {
        Value = context.ReadValue<Vector2>();
        Invoke(Value);
    }
}