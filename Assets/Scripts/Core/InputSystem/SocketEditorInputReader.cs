using UnityEngine;
using UnityEngine.InputSystem;


[CreateAssetMenu(menuName = "Input Reader/Socket Editor", fileName = "SocketEditorReader", order = 0)]
public class SocketEditorInputReader : InputReader<SocketEditorInput>, SocketEditorInput.IActionsActions
{
    public Vector2InputData Move;
    public Vector2InputData Look;
    public BoolInputData CreateSocket;
    public BoolInputData DeleteSocket;
    public BoolInputData ActivateCameraRotation;
    public BoolInputData ActivateCameraPan; 
    public BoolInputData Select;
    public FloatInputData ForwardMovement; 
    
    public Vector3 MousePosition => Mouse.current.position.ReadValue();
    public Ray MouseRay => Camera.main.ScreenPointToRay(MousePosition);
    
    protected override void OnEnableInput(InputActionType inputActionType)
    {
        if (inputActionType == InputActionType.Freelook) InputActions.Actions.Enable();
    }

    protected override void OnInputActionsCreated()
    {
        InputActions.Actions.SetCallbacks(this); 
        
        Move = new Vector2InputData(InputActions.Actions.Move);
        Look = new Vector2InputData(InputActions.Actions.Look);
        CreateSocket = new BoolInputData(InputActions.Actions.CreateSocket);
        DeleteSocket = new BoolInputData(InputActions.Actions.DeleteSocket);
        ActivateCameraRotation = new BoolInputData(InputActions.Actions.ActivateCameraRotation);
        ActivateCameraPan = new BoolInputData(InputActions.Actions.ActiveCameraPan);
        Select = new BoolInputData(InputActions.Actions.Select);
        ForwardMovement = new FloatInputData(InputActions.Actions.ForwardMovement);
    }

    public void OnMove(InputAction.CallbackContext context) => Move.Trigger(context);
    public void OnLook(InputAction.CallbackContext context) => Look.Trigger(context);
    public void OnCreateSocket(InputAction.CallbackContext context) => CreateSocket.Trigger(context);
    public void OnDeleteSocket(InputAction.CallbackContext context) => DeleteSocket.Trigger(context);
    public void OnForwardMovement(InputAction.CallbackContext context) => ForwardMovement.Trigger(context);
    public void OnActivateCameraRotation(InputAction.CallbackContext context) => ActivateCameraRotation.Trigger(context);
    public void OnActiveCameraPan(InputAction.CallbackContext context) => ActivateCameraPan.Trigger(context);
    public void OnSelect(InputAction.CallbackContext context) => Select.Trigger(context);
}