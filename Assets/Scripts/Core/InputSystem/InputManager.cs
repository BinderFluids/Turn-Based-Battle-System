using System;
using UnityEngine;
using UnityUtils;

public class InputManager : Singleton<InputManager>
{
    [SerializeField] private bool enableInputOnAwake; 
    [SerializeField] private InputReader inputReader;
    public InputReader InputReader => inputReader;

    protected override void Awake()
    {
        base.Awake();
        if (enableInputOnAwake) EnableInput();
    }

    
    public void EnableInput() => inputReader.EnableInput(InputActionType.Player);
}
