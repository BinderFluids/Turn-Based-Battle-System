using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityUtils; 

public class SelectionManager : Singleton<SelectionManager>
{
    [SerializeField] private int selectionCount;
    
    private List<ISelectable> activeItems;

    public Observer<ISelectable> CurrentItem; 
    private bool active = false; 

    int GetTrueIndex(int index, List<ISelectable> items)
    {
        if (items.Count == 0) return 0; 
        int newIndex = (index % items.Count + items.Count) % items.Count;
        return newIndex;
    }
    
    public void StartSelection(List<ISelectable> items, int index = 0)
    {
        if (active)
        {
            Debug.LogWarning("Selection already started");
            return; 
        }

        active = true; 
        activeItems = items; 
        selectionCount = index;
        CurrentItem.Value = activeItems[selectionCount];
    }

    public void EndSelection()
    {
        if (!active)
        {
            Debug.LogWarning("No selection session to end");
            return;
        }

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
            EventBus<SelectableChosenEvent>.Raise(
                new SelectableChosenEvent
                {
                    SelectedItem = CurrentItem.Value
                }
            ); 
            EndSelection();
        }
        
    }

    void NavigateSelectables()
    {
        if (Input.GetKeyDown(KeyCode.D))
        {
            ISelectable previousItem = CurrentItem.Value;
            selectionCount++;
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            ISelectable previousItem = CurrentItem.Value;
            selectionCount--;
        }
        var index = GetTrueIndex(selectionCount, activeItems);
        CurrentItem.Value = activeItems[index];
    }
}
