using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityUtils; 

public class SelectionManager : Singleton<SelectionManager>
{
    [SerializeField] private int selectionCount;
    
    private List<ISelectable> activeItems;
    public Observer<ISelectable> CurrentItem = new Observer<ISelectable>(null); 
    private bool active = false;

    [SerializeField] private SelectionHighlighter defaultHighligher;
    [SerializeField] private SelectionHighlighter currentHighlighter; 
    
    int GetTrueIndex(int index, List<ISelectable> items)
    {
        if (items.Count == 0) return 0; 
        int newIndex = (index % items.Count + items.Count) % items.Count;
        return newIndex;
    }
    
    public void StartSelection(List<ISelectable> items, int index = 0, SelectionHighlighter highlighter = null)
    {
        print($"Try Start Selection with {items.Count} items");
        if (active)
        {
            Debug.LogWarning("Selection already started");
            return; 
        }

        if (highlighter == null)
            currentHighlighter ??= defaultHighligher;
        else
            currentHighlighter = highlighter;
        
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
        
        if (Input.GetKeyDown(KeyCode.Space))
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
        if (Input.GetKeyDown(KeyCode.D)) ShiftSelection(1);
        if (Input.GetKeyDown(KeyCode.A)) ShiftSelection(-1); 
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
