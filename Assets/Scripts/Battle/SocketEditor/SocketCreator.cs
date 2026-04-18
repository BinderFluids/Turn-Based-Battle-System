using System;
using UnityEngine;
using UnityEngine.Events;

public class SocketCreator : MonoBehaviour
{
    [SerializeField] private Camera cam; 
    [SerializeField] private SocketHandle socketPrefab;
    [SerializeField] private SocketEditorInputReader input;
    [SerializeField] private UnityEvent<SocketHandle> onSocketCreated;
    
    private void Update()
    {
        if (input.CreateSocket.WasPressedThisFrame)
        {
            if (!Physics.Raycast(input.MouseRay, out var hit)) return;
            CreateSocket(hit.point);
        }
    }

    public SocketHandle CreateSocket(Vector3 position)
    {
        var newSocketHandle = Instantiate(socketPrefab, position, Quaternion.identity);
        onSocketCreated?.Invoke(newSocketHandle); 
        return newSocketHandle;
    }
}
