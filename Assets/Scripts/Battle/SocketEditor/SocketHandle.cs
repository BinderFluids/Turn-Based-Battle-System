using System;
using TransformHandles;
using UnityEngine;
using UnityEngine.Events;

public class SocketHandle : MonoBehaviour
{
    private Handle handle;
    private event Action<Handle> selectHandle;
    
    public void Initialize(string name, Handle handle)
    {
        gameObject.name = name;
        this.handle = handle;
        
        selectHandle = _ => SelectThisHandle(); 
        handle.OnInteractionStartEvent += selectHandle; 
    }

    private void OnDestroy()
    {
        handle.OnInteractionStartEvent -= selectHandle; 
    }

    public void SnapToGround()
    {
        SetPosition(new Vector3(transform.position.x, 0, transform.position.z));
    }

    void SetPosition(Vector3 position)
    {
        transform.position = position;
        handle.target.position = position; 
    }

    void SelectThisHandle()
    {
        Debug.Log("selc");
        SocketCursor.Instance.SelectSocket(this); 
    }
}
