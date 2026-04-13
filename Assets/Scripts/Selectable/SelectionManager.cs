using EventBus; 
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
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

    private BoolInputData selectInputData;
    private Vector2InputData navigateInputData;
    

    int GetTrueIndex(int index, List<ISelectable> items)
    {
        if (items.Count == 0) return 0; 
        int newIndex = (index % items.Count + items.Count) % items.Count;
        return newIndex;
    }

    public void Configure(BoolInputData selectInputData, Vector2InputData navigateInputData)
    {
        this.selectInputData = selectInputData;
        this.navigateInputData = navigateInputData;
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
        
        if (selectInputData.WasPressedThisFrame)
        {
            CurrentItem.Value.Select();
            EndSelection();
            EventBus<SelectableChosenEvent>.Raise(
                new SelectableChosenEvent
                {
                    SelectedItem = CurrentItem.Value
                }
            ); 
        }
    }

    void NavigateSelectables()
    {
        if (!navigateInputData.WasPressedThisFrame) return; 
        
        if (navigateInputData.LeftWasPressedThisFrame)
            ShiftSelection(-1);
        if (navigateInputData.RightWasPressedThisFrame)
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
