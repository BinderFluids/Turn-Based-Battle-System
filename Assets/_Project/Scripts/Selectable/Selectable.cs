using System;

public interface ISelectable
{ 
    event Action OnSelected;
    void Select();
    void OnHover(bool isHovering);
}