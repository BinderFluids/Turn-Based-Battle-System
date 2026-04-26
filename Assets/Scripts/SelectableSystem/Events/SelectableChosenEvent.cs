using EventBus;

namespace SelectableSystem.Events
{
    public struct SelectableChosenEvent : IEvent
    {
        public ISelectable SelectedItem;
    }
}
