using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SocketReference
{
    [SerializeField] private SocketData socketData;
    [SerializeField, HideInInspector] private string selectedSocketName; 

    public Vector3 GetSocketPosition() => socketData.GetSocketPositionsDictionary()[selectedSocketName];
}