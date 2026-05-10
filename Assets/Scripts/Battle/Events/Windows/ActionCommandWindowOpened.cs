using EventBus;

namespace Battle.Events.Windows
{
    /// <summary>
    /// Raised when a timed action command window starts. UI should show prompts and timing feedback.
    /// </summary>
    public struct ActionCommandWindowOpened : IEvent
    {
        public string WindowId { get; }
        public float DurationSeconds { get; }

        public ActionCommandWindowOpened(string windowId, float durationSeconds)
        {
            WindowId = windowId;
            DurationSeconds = durationSeconds;
        }
    }
}