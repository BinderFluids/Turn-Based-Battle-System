using System;
using Cysharp.Threading.Tasks;
using PrimeTween;
using UnityEngine;

public class FollowSelection : SelectionHighlighter
{
    private ISelectable currentSelection;
    [SerializeField] private Transform target;
    [SerializeField] private Renderer renderer; 
    
    [SerializeField] private float time;
    [SerializeField] private Vector3 offset;
    [SerializeField] private Ease ease;
    private Tween tween;

    private void Start()
    {
        OnDeactivate();
    }

    protected override void SelectionChanged(ISelectable newSelection)
    {
        currentSelection = newSelection;
        if (newSelection is Component component)
            target = component.transform;
        else
            target = null;

        tween = Tween.Position(transform, target.position + offset, time, ease); 
    }

    protected override void OnActivate()
    {
        //transform.position = target.position + offset;
        renderer.enabled = true; 
    }
    protected override void OnDeactivate()
    {
        renderer.enabled = false; 
    }
}