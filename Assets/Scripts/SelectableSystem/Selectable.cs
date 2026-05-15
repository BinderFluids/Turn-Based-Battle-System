using System;
using UnityEngine; 

namespace SelectableSystem
{
    public interface ISelectable
    { 
        Vector3 SelectionAnchor { get;  }
        
        event Action OnSelected;
        void Select();
    }
}