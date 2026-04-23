using System;
using Battle.Components;

namespace Battle.SelectionStrategy
{
    public class SelectableComponent : BattleEntityComponent, ISelectable
    {
        public event Action OnSelected;
        public void Select()
        {
            //noop
        }
    }
}