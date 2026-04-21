using System.Collections.Generic;
using Battle.SocketEditor;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class SocketEditorManager : Singleton<SocketEditorManager>
{
    [SerializeField] private SocketCreator creator;
    [SerializeField] private SocketPositionMapGenerator generator;
    
    [SerializeField] private SocketHandle currentSocket;
    [SerializeField] private SocketPositionMap currentSocketMap;
    [SerializeField] private SocketPositionMap defaultSocketMap;
    public SocketPositionMap CurrentSocketMap => currentSocketMap;
    [SerializeField] private TMP_Text socketMapNameText;
    [SerializeField] private TMP_Text saveAsNameText; 
     
    public SocketHandle CurrentSocket => currentSocket;
    [SerializeField] private UnityEvent<SocketHandle> onSocketSelected;

    [SerializeField] private List<SocketHandle> sockets = new(); 
    public IReadOnlyList<SocketHandle> Sockets => sockets;

    public void SaveAs() => SetCurrentSocketMap(generator.Generate(saveAsNameText.text, sockets));
    public void Save() => generator.Generate(currentSocketMap.name, sockets);


    private void Start()
    {
        creator.onSocketCreated.AddListener(HandleSocketCreated);
        if (defaultSocketMap == null)
            SetCurrentSocketMap(generator.Generate("Default", sockets));
        else
            SetCurrentSocketMap(defaultSocketMap);
    }

    public void SetCurrentSocketMap(SocketPositionMap map)
    {
        currentSocketMap = map;
        socketMapNameText.text = map.name; 
        PopulateSockets(map);
    }

    public void PopulateSockets(SocketPositionMap map)
    {
        ClearSockets();
        foreach (var kvp in map.GetSocketPositionsDictionary())
            creator.CreateSocket(kvp.Value, kvp.Key); 
    }

    public void ClearSockets()
    {
        for (int i = sockets.Count - 1; i >= 0; i--)
            Destroy(sockets[i].gameObject);
        sockets.Clear();
    }
    
    public void SetCurrentSocket(SocketHandle socket)
    {
        currentSocket = socket;
        onSocketSelected?.Invoke(socket);
    }
    
    void HandleSocketCreated(SocketHandle newSocket)
    {
        sockets.Add(newSocket); 
    }
    
    protected override void OnDestroy()
    {
        base.OnDestroy();
        creator.onSocketCreated.RemoveListener(HandleSocketCreated);
    }
}