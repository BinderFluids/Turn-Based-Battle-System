using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

public abstract class SelectionHighlighter : MonoBehaviour
{
    public void Activate()
    {
        SelectionManager.Instance.CurrentItem.onValueChanged += SelectionChanged; 
        OnActivate();
    }

    protected virtual void OnActivate() { }

    protected abstract void SelectionChanged(ISelectable newSelection);

    public virtual UniTask GetHighlightTask()
    {
        return UniTask.CompletedTask;
    } 

    public void Deactivate()
    {
        SelectionManager.Instance.CurrentItem.onValueChanged -= SelectionChanged; 
        OnDeactivate();
    }
    protected virtual void OnDeactivate() { }
    
    private void OnDestroy()
    {
        Deactivate();
    }
}