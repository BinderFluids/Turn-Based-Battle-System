
using EventBus;

public struct TurnEndEvent : IEvent
{
    public TurnComponent turnEntity; 
}