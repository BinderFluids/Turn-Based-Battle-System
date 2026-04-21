using UnityEngine;
using EventBus; 

public class SelectActionTurnHandle : ITurnHandleStrategy
{
    TurnComponent turnComponent;
    ActorComponent actionComponent;
    
    public void Handle(TurnComponent component)
    {
        turnComponent = component;
        if (!component.TryGetComponent(out actionComponent))
        {
            Debug.LogWarning($"{component.gameObject.name}: No action container component found on turn component");
            ActionEnded();
            return; 
        }

        actionComponent.onActionEnded += ActionEnded; 
        actionComponent.SelectAction();
    }

    void ActionEnded()
    {
        if (actionComponent != null)
            actionComponent.onActionEnded -= ActionEnded; 
        
        TurnEndEvent turnEndEvent = new TurnEndEvent {turnEntity = turnComponent};
        EventBus<TurnEndEvent>.Raise(turnEndEvent);
    }
}

public class NextTurnHandle : ITurnHandleStrategy
{
    public void Handle(TurnComponent component)
    {
        
    }
}