
using EventBus;

public struct NextTurnEvent : IEvent
{
    public BattleEntity previousActor;
}