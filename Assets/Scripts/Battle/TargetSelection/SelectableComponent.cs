using System;
using Battle.Enums;
using SelectableSystem;

namespace Battle.TargetSelection
{
    public class SelectableComponent : BattleEntityComponent, ISelectable
    {
        protected override ComponentType componentType => ComponentType.Selectable; 
        
        public event Action OnSelected;
        public void Select()
        {
            //noop
        }

    }
}