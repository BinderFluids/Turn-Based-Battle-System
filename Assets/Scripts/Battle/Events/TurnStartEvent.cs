using EventBus;

public struct TurnStartEvent : IEvent
{
    public TurnComponent turnEntity; 
}