using EventBus;

/// <summary>
/// Used for Action Commands, and Responsive Windows
/// </summary>
public class WindowInputEvent : IEvent
{
    public Battle.Input.InputType InputType { get; }
    public float TimeSeconds { get; }
    public WindowInputEvent(Battle.Input.InputType inputType, float timeSeconds)
    {
        InputType = inputType;
        TimeSeconds = timeSeconds;
    }
}