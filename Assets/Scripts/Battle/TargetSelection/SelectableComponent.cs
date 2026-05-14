using System;
using Battle.Enums;
using SelectableSystem;
using UnityEngine;
using RequestHub; 
using Battle.Requests;

namespace Battle.TargetSelection
{
    public class SelectableComponent : BattleEntityComponent, ISelectable
    {
        protected override ComponentType componentType => ComponentType.Selectable;
        
        public Vector3 SelectionAnchor 
        {
            get
            {
                // if (RequestHub<RequestFormationAnchors>.TryRequest(Entity, out var request))
                //     if (request.Offsets.TryGetPosition(FormationOffsets.Top, out var topPosition))
                //         return topPosition;
                //
                
                return transform.position;
            }            
        }
        
        public event Action OnSelected;
        public void Select()
        {
            //noop
        }

    }
}