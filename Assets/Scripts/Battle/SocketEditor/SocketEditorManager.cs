
using UnityUtils;
using UnityEngine;
using UnityEngine.Events;

public class SocketEditorManager : Singleton<SocketEditorManager>
{
    [SerializeField] private SocketHandle currentSocket;
    public SocketHandle CurrentSocket => currentSocket;
    [SerializeField] private UnityEvent<SocketHandle> onSocketSelected;
    
    public void SetCurrentSocket(SocketHandle socket)
    {
        currentSocket = socket;
        onSocketSelected?.Invoke(socket);
    } 
}