
using System;

public partial class BattleEntity : ISelectable
{
    //ISelection Implementation
    public event Action OnSelected;
    public void Select()
    {
        //throw new NotImplementedException();
    }
}