using System;
using SelectableSystem;

namespace Battle.TargetSelection
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