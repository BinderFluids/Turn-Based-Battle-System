using System;
using SerializedInterface;
using UnityEngine;
using UnityUtils;

public class InputManager : Singleton<InputManager>
{
    [SerializeField] private bool enableInputOnAwake; 
    [SerializeField] private InterfaceReference<IInputReader> inputReaderReference;
    public IInputReader InputReader => inputReaderReference.Value;

    protected override void Awake()
    {
        base.Awake();
        if (enableInputOnAwake) EnableInput();
    }

    
    public void EnableInput() => InputReader.EnableInput(InputActionType.Player);
}
