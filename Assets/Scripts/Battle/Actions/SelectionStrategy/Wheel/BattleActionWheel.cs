
using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityUtils;

public class BattleActionWheel : MonoBehaviour, IBattleActionSelectionStrategy
{
    [SerializeField] List<BattleActionWheelItem> items;
    
    private EventBinding<SelectableChosenEvent> chosenItemBinding;
    public event Action<IBattleAction> onActionSelected;
    private BattleActionWheelItem selectedItem; 

    private void Start()
    {
        chosenItemBinding = new EventBinding<SelectableChosenEvent>
            (OnSelectableChosenEventRaised); 
    }

    public void GetAction(List<IBattleAction> context)
    {
        EventBus<SelectableChosenEvent>.Register(chosenItemBinding); 
        
        ActivateItems();
        SelectionManager.Instance.StartSelection(
            items.ConvertAll(i => i as ISelectable
            )); 
    }
    
    void OnSelectableChosenEventRaised(SelectableChosenEvent @event)
    {
        selectedItem = (BattleActionWheelItem)@event.SelectedItem; 
        onActionSelected?.Invoke(selectedItem.Action); 
        EventBus<SelectableChosenEvent>.Deregister(chosenItemBinding); 
        ActivateItems(false); 
    }
    
    void ActivateItems(bool active = true)
    {
        foreach (var item in items)
            item.gameObject.SetActive(active); 
    }
}