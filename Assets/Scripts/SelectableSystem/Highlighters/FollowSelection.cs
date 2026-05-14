
using LitMotion;
using LitMotion.Extensions;
using SelectableSystem;
using UnityEngine;

namespace Highlighters
{
    public class FollowSelection : SelectionHighlighter
    {
        private ISelectable currentSelection;
        [SerializeField] private Transform target;
        [SerializeField] private Renderer renderer; 
    
        [SerializeField] private float time;
        [SerializeField] private Vector3 offset;
        [SerializeField] private Ease ease;

        private void Start()
        {
            OnDeactivate();
        }

        protected override void SelectionChanged(ISelectable newSelection)
        {
            currentSelection = newSelection;
            if (newSelection is Component component)
            {
                target = component.transform;
                Vector3 startPosition = transform.position;
            
                LMotion.Create(startPosition, target.position + offset, time)
                    .BindToPosition(transform)
                    .AddTo(transform);
            }
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
}