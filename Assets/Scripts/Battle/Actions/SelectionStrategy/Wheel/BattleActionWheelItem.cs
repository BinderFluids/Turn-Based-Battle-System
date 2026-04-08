using System;
using UnityEngine;

public class BattleActionWheelItem : MonoBehaviour, ISelectable
{
    [SerializeField] private InterfaceReference<IBattleAction> actionRef;
    public IBattleAction Action => actionRef.Value;
    
    public event Action OnSelected;
    public void Select() => OnSelected?.Invoke();

    public void OnHover(bool hovering)
    {
        if (!hovering) return;
        Debug.Log($"Hovering over: {gameObject.name}");
    }
}