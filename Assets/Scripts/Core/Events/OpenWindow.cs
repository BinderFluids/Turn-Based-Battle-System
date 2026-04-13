using EventBus;

public class OpenWindow : IEvent
{
    public float DurationSeconds { get; }
    public string WindowId { get; }

    public OpenWindow(float durationSeconds, string windowId = null)
    {
        DurationSeconds = durationSeconds;
        WindowId = windowId;
    }
}
