
using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityUtils;

public class BattleActionWheel : MonoBehaviour, IBattleActionSelectionStrategy
{
    [SerializeField] List<BattleActionWheelItem> items;
    
    private EventBinding<SelectableChosenEvent> chosenItemBinding;
    private bool eventTriggered;
    private BattleActionWheelItem selectedItem; 

    private void Start()
    {
        eventTriggered = false; 
        chosenItemBinding = new EventBinding<SelectableChosenEvent>
            (OnSelectableChosenEventRaised); 
    }

    void OnSelectableChosenEventRaised(SelectableChosenEvent @event)
    {
        selectedItem = (BattleActionWheelItem)@event.SelectedItem; 
        eventTriggered = true;
    }

    public async UniTask<IBattleAction> GetAction(List<IBattleAction> context)
    {
        EventBus<SelectableChosenEvent>.Register(chosenItemBinding); 
        
        ActivateItems();
        SelectionManager.Instance.StartSelection(
            items.ConvertAll(i => i as ISelectable
            )); 
        
        await UniTask.WaitUntil(() => eventTriggered);
        eventTriggered = false;

        ActivateItems(false); 
        EventBus<SelectableChosenEvent>.Deregister(chosenItemBinding); 
        
        return selectedItem.Action;
    }
    
    void ActivateItems(bool active = true)
    {
        foreach (var item in items)
            item.gameObject.SetActive(active); 
    }
}