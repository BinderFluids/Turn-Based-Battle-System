using System;
using TransformHandles;
using UnityEngine;
using UnityEngine.Events;

public class SocketHandle : MonoBehaviour
{
    public void Initialize(string name)
    {
        gameObject.name = name;
    }

    public void SnapToGround()
    {
        SetPosition(new Vector3(transform.position.x, 0, transform.position.z));
    }

    void SetPosition(Vector3 position)
    {
        transform.position = position;
    }

    void SelectThisHandle()
    {
        Debug.Log("selc");
        SocketCursor.Instance.SelectSocket(this); 
    }
}
