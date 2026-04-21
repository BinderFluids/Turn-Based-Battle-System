using UnityEngine;

[System.Serializable]
public class SocketReference
{
    [SerializeField] private SocketPositionMap socketPositionMap;
    [SerializeField, HideInInspector] private string selectedSocketName; 

    public Vector3 GetSocketPosition() => socketPositionMap.GetSocketPositionsDictionary()[selectedSocketName];
}