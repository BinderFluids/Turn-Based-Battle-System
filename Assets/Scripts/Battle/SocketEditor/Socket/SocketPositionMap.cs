using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class SocketPositionMap : ScriptableObject
{
    [SerializeReference] private List<string> m_socketNames = new();
    [SerializeReference] private List<Vector2> m_socketPositions = new();

#if UNITY_EDITOR
    public void Clear()
    {
        Debug.Log($"Clearing SocketData from {name}");
        m_socketNames.Clear();
        m_socketPositions.Clear();
    }

    public void AddSocket(string socketName, Vector2 socketPosition)
    {
        EditorUtility.SetDirty(this);

        if (socketName == null)
            throw new ArgumentNullException(nameof(socketName));
        if (socketName == String.Empty)
            throw new ArgumentException("Socket name cannot be empty");
        if (m_socketNames.Contains(socketName))
            throw new ArgumentException($"Socket name '{socketName}' already exists");
        
        m_socketNames.Add(socketName);
        m_socketPositions.Add(socketPosition);
    }
#endif

    public Dictionary<string, Vector2> GetSocketPositionsDictionary()
    {
        if (m_socketNames.Count != m_socketPositions.Count)
            throw new ArgumentException("Socket names and positions must be the same length");
        
        Dictionary<string, Vector2> socketPositions = 
            m_socketNames.Zip(m_socketPositions, (socketName, pos) => new {socketName, pos})
            .ToDictionary(x => x.socketName, x => x.pos);
        
        return socketPositions;
    }
}