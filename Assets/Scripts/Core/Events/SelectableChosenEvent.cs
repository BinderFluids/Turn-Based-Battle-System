using EventBus; 

public struct SelectableChosenEvent : IEvent
{
    public ISelectable SelectedItem;
}