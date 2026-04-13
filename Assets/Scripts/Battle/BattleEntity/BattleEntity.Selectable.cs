using System;

public partial class BattleEntity : ISelectable
{
    public event Action OnSelected;
    public void Select()
    {
        throw new NotImplementedException();
    }
}