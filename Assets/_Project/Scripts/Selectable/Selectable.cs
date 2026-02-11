using System;

public interface ISelectable
{ 
    event Action OnSelected;
    void Select();
}

public interface IHoverable
{
    void OnHover(bool isHovering);
}