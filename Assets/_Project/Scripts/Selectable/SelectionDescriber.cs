using UnityEngine;

public class SelectionDescriber : SelectionHighlighter
{
    protected override void SelectionChanged(ISelectable newSelection)
    {
        if (newSelection is Component component)
        {
            Debug.Log($"Selecting {component.gameObject.name}"); 
        }
    }
}