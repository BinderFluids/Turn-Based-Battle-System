using EventBus;

/// <summary>
/// Used for Action Commands, and Responsive Windows
/// </summary>
public class WindowInputEvent : IEvent
{
    public string WindowId { get; }
    public Battle.Input.InputType InputType { get; }
    public float TimeSeconds { get; }
    public WindowInputEvent(string windowId, Battle.Input.InputType inputType, float timeSeconds)
    {
        WindowId = windowId;
        InputType = inputType;
        TimeSeconds = timeSeconds;
    }
}