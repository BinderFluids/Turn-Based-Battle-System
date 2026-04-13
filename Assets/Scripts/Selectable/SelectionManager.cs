using System;
using EventBus; 
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Selectable;
using UnityEngine;
using UnityUtils; 

public class SelectionManager : Singleton<SelectionManager>
{
    [SerializeField] private int selectionCount;
    
    private List<ISelectable> activeItems;
    public Observer<ISelectable> CurrentItem = new Observer<ISelectable>(null); 
    private bool active = false;

    [SerializeField] private SelectionHighlighter defaultHighligher;
    [SerializeField] private SelectionHighlighter currentHighlighter;

    [SerializeField] private InterfaceReference<ISelectableInput> input; //TODO THIS IS A PLACHOLDER
    private ISelectableInput Input => input.Value;
    

    int GetTrueIndex(int index, List<ISelectable> items)
    {
        if (items.Count == 0) return 0; 
        int newIndex = (index % items.Count + items.Count) % items.Count;
        return newIndex;
    }

    public void StartSelection(
        List<ISelectable> items, 
        int index = 0, 
        SelectionHighlighter highlighter = null
        )
    {
        print($"Try Start Selection with {items.Count} items");
        if (active)
        {
            Debug.LogWarning("Selection already started");
            return; 
        }

        currentHighlighter = highlighter ?? defaultHighligher; 
        
        active = true; 
        activeItems = items; 
        selectionCount = index;
        
        currentHighlighter.Activate();
        CurrentItem.Value = activeItems[selectionCount];
    }

    public void EndSelection()
    {
        if (!active)
        {
            Debug.LogWarning("No selection session to end");
            return;
        }

        currentHighlighter.Deactivate();
        currentHighlighter = null; 
        
        active = false; 
        activeItems.Clear();
    }

    private void Update()
    {
        if (!active) return;

        NavigateSelectables();
        
        if (Input.Confirm.WasPressedThisFrame)
        {
            CurrentItem.Value.Select();
            EndSelection();
            
            EventBus<SelectableChosenEvent>.Raise( //TODO THIS IS KILLING ME MAN HELP ME
                new SelectableChosenEvent
                {
                    SelectedItem = CurrentItem.Value
                }
            ); 
            
            Debug.Log("SelectableChosenEvent.Raise() Successful");
        }
    }

    void NavigateSelectables()
    {
        if (!Input.Navigate.WasPressedThisFrame) return; 
        
        if (Input.Navigate.LeftWasPressedThisFrame)
            ShiftSelection(-1);
        if (Input.Navigate.RightWasPressedThisFrame)
            ShiftSelection(1);
    }

    private UniTask currentHighlighterTask; 
    void ShiftSelection(int shift)
    {
        //if (currentHighlighterTask.Status == UniTaskStatus.Pending) return;
        
        selectionCount += shift; 
        var index = GetTrueIndex(selectionCount, activeItems);
        CurrentItem.Value = activeItems[index];

        //currentHighlighterTask = currentHighlighter.GetHighlightTask();
    }
}
