using System;
using UnityEditor.Experimental.GraphView;

public class SelectableComponent : BattleEntityComponent, ISelectable
{
    public event Action OnSelected;
    public void Select()
    {
        //noop
    }
}