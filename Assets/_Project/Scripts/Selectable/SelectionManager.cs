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
    public ISelectable SelectedItem => activeItems[GetTrueIndex(selectionCount, activeItems)];
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
        SelectedItem.OnHover(true); 
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
        
        if (Input.GetKeyDown(KeyCode.D))
        {
            ISelectable previousItem = SelectedItem;
            selectionCount++;
            
            if (previousItem != SelectedItem) previousItem.OnHover(false);
            SelectedItem.OnHover(true); 
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            ISelectable previousItem = SelectedItem;
            selectionCount--;
            
            if (previousItem != SelectedItem) previousItem.OnHover(false);
            SelectedItem.OnHover(true); 
        }

        GetTrueIndex(selectionCount, activeItems);
        
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SelectedItem.Select();
            EventBus<SelectableChosenEvent>.Raise(
                new SelectableChosenEvent
                {
                    SelectedItem = SelectedItem
                }
            ); 
            EndSelection();
        }
        
    }
}