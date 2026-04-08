
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
    public override event Action<BattleEntity> onEntitySelected;
    
    private EventBinding<SelectableChosenEvent> chosenEventBinding;
    [SerializeField] private List<PhysicalBattleEntityModifier> modifiers;
    
    public override void GetEntity(BattleEntity actor, IBattleAction action)
    {
        Debug.Log("Manual select target");
        chosenEventBinding ??= new EventBinding<SelectableChosenEvent>(OnSelectableChosenEventRaised);
        
        EventBus<SelectableChosenEvent>.Register(chosenEventBinding);
        
        //Create list of selectable entities
        List<ISelectable> entitiesAsSelectables = new();
        action.GetValidTargets(actor).ForEach(e => entitiesAsSelectables.Add(e as ISelectable));
        SelectionManager.Instance.StartSelection(entitiesAsSelectables);
    }
    
    void OnSelectableChosenEventRaised(SelectableChosenEvent e)
    {
        BattleEntity selectedEntity = e.SelectedItem as BattleEntity;
        onEntitySelected?.Invoke(selectedEntity);
        
        EventBus<SelectableChosenEvent>.Deregister(chosenEventBinding);
    }
}