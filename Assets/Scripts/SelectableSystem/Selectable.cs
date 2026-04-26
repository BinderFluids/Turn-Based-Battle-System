using System;

namespace SelectableSystem
{
    public interface ISelectable
    { 
        event Action OnSelected;
        void Select();
    }
}