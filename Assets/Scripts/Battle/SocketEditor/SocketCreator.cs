using System;
using System.Net.Sockets;
using TransformHandles;
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

    public void CreateSocket(Vector3 position)
    {
        SocketHandle newSocketHandle = Instantiate(socketPrefab, position, Quaternion.identity);  
        GameObject socketHandleGameObject = newSocketHandle.gameObject;
        
        newSocketHandle.Initialize(socketHandleGameObject.name);
        
        onSocketCreated?.Invoke(newSocketHandle);
    }
}
