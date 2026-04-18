
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class SocketCursor : Singleton<SocketCursor>
{
    [SerializeField] private UnityEvent<SocketHandle> onSocketSelected;
    [SerializeField] private SocketEditorInputReader input; 
    
    private void Update()
    {
        if (EventSystem.current.IsPointerOverGameObject()) return; 
        
        if (input.Select.WasPressedThisFrame)
        {
            if (!Physics.Raycast(input.MouseRay, out var hit))
            {
                SelectSocket(null); 
                return;
            }
            if (!hit.collider.TryGetComponent(out SocketHandle socketHandle))
            {
                SelectSocket(null); 
                return;
            }

            SelectSocket(socketHandle); 
        }
    }

    public void SelectSocket(SocketHandle socketHandle)
    {
        if (socketHandle == null) return; 
        onSocketSelected?.Invoke(socketHandle);
    }
}