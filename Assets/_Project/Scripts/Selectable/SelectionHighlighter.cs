using System;
using UnityEngine;

public abstract class SelectionHighlighter : MonoBehaviour
{
    public void Activate()
    {
        SelectionManager.Instance.CurrentItem.onValueChanged += SelectionChanged; 
    }
    
    protected abstract void SelectionChanged(ISelectable newSelection);
    
    public void Deactivate()
    {
        SelectionManager.Instance.CurrentItem.onValueChanged -= SelectionChanged; 
    }
    private void OnDestroy()
    {
        Deactivate();
    }
}