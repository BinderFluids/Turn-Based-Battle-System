
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Registry;
using UnityUtils;


[CreateAssetMenu(menuName = "Battle Entity Selection Strategy/Manual", fileName = "ManualSelectTarget", order = 0)]
public class ManualSelectTarget : ScriptableBattleEntitySelectionStrategy
{
    private bool eventTriggered; 
    private EventBinding<SelectableChosenEvent> chosenEventBinding;
    private SelectableChosenEvent chosenEvent;

    void OnSelectableChosenEventRaised(SelectableChosenEvent e)
    {
        eventTriggered = true;
        chosenEvent = e;
    }
    
    public override async UniTask<BattleEntity> GetEntity(BattleEntity actor, CancellationToken ct)
    {
        if (chosenEventBinding == null)
            chosenEventBinding = new EventBinding<SelectableChosenEvent>(OnSelectableChosenEventRaised);
        
        EventBus<SelectableChosenEvent>.Register(chosenEventBinding);

        //Create list of selectable entities
        List<ISelectable> entitiesAsSelectables = new();
        Registry<BattleEntity>
            .All
            .Where(e => e != actor)
            .ForEach(e => entitiesAsSelectables.Add(e));
        SelectionManager.Instance.StartSelection(entitiesAsSelectables);
        
        eventTriggered = false;
        while (!eventTriggered)
        {
            if (Input.GetKeyDown(KeyCode.Z))
                actor.CancelTargetSelection();

            if (ct.IsCancellationRequested)
            {
                Debug.Log("Cancelling target selection");
                
                SelectionManager.Instance.EndSelection();
                EventBus<SelectableChosenEvent>.Deregister(chosenEventBinding);
                actor.StartTurn().Forget(); 
                
                ct.ThrowIfCancellationRequested();
                return null;
            }

            await UniTask.Yield(); 
        }
        
        EventBus<SelectableChosenEvent>.Deregister(chosenEventBinding);
        return (BattleEntity)chosenEvent.SelectedItem;
    }
}