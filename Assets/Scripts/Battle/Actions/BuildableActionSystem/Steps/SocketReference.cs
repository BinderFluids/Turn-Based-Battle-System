using UnityEngine;

[System.Serializable]
public class SocketReference
{
    [SerializeField, HideInInspector] private SocketData m_socketData;
    [SerializeField, HideInInspector] private string m_selectedSocketName; 
    public string selectedSocketName => m_selectedSocketName;
    [SerializeField, HideInInspector] private int m_hiddenValidateValue;
    
    public void Update(SocketData socketData)
    {
        m_socketData = socketData;
    }
    
    public SocketReference(SocketData dataSource)
    {
        m_socketData = dataSource;
    }
}