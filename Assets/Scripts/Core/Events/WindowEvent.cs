using EventBus;

public class WindowEvent : IEvent
{
    // TODO: Here we need to get some type of window input
    public string WindowId { get; }

    public WindowEvent(string windowId = null)
    {
        WindowId = windowId;
    }
}