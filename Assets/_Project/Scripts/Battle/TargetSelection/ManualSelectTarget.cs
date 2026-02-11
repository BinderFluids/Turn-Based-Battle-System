
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
    [SerializeField] private List<PhysicalBattleEntityModifier> modifiers;

    void OnSelectableChosenEventRaised(SelectableChosenEvent e)
    {
        eventTriggered = true;
        chosenEvent = e;
    }
    
    public override async UniTask<BattleEntity> GetEntity(BattleEntity actor, IBattleAction action, CancellationToken ct)
    {
        if (chosenEventBinding == null)
            chosenEventBinding = new EventBinding<SelectableChosenEvent>(OnSelectableChosenEventRaised);
        
        EventBus<SelectableChosenEvent>.Register(chosenEventBinding);

        //Create list of selectable entities
        List<ISelectable> entitiesAsSelectables = new();
        action.GetValidTargets(actor).ForEach(e => entitiesAsSelectables.Add(e as ISelectable));
        SelectionManager.Instance.StartSelection(entitiesAsSelectables);
        
        //Wait for SelectionManager to make a selection
        eventTriggered = false;
        while (!eventTriggered)
        {
            //Throw cancellation exception if Z is pressed
            if (Input.GetKeyDown(KeyCode.Z))
                actor.CancelTargetSelection();

            if (ct.IsCancellationRequested)
            {
                Debug.Log("Cancelling target selection");
                
                SelectionManager.Instance.EndSelection();
                EventBus<SelectableChosenEvent>.Deregister(chosenEventBinding);
                
                ct.ThrowIfCancellationRequested();
                return null;
            }

            await UniTask.Yield(); 
        }
        
        EventBus<SelectableChosenEvent>.Deregister(chosenEventBinding);
        return (BattleEntity)chosenEvent.SelectedItem;
    }
}