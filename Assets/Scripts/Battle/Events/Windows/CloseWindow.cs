using EventBus;

namespace Battle.Events.Windows
{
    public class CloseWindow : IEvent
    {
        public string WindowId { get; }

        public CloseWindow(string windowId = null)
        {
            WindowId = windowId;
        }
    }
}
