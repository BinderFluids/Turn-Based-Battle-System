
using System;
using TMPro;
using UnityEngine;

public class SocketWindow : MonoBehaviour
{
    [SerializeField] private GameObject windowContainer;
    [SerializeField] private TMP_InputField socketNameField; 

    private void Start()
    {
        socketNameField.onValueChanged.AddListener(UpdateCurrentSocketName);
    }
    private void OnDestroy()
    {
        socketNameField.onValueChanged.RemoveListener(UpdateCurrentSocketName);
    }

    void UpdateCurrentSocketName(string newName)
    {
        if (SocketEditorManager.Instance.CurrentSocket == null) return; 
        SocketEditorManager.Instance.CurrentSocket.name = newName;
    }

    public void Open(SocketHandle socketHandle)
    {
        if (socketHandle == null)
        {
            Close();
            return; 
        }
        
        windowContainer.SetActive(true);
        socketNameField.text = SocketEditorManager.Instance.CurrentSocket.name;
    }

    public void Close()
    {
        windowContainer.SetActive(false);
    }

    public void SnapSocketToGround()
    {
        Vector3 socketPos = SocketEditorManager.Instance.CurrentSocket.transform.position;
        SocketEditorManager.Instance.CurrentSocket.SnapToGround();
    }
}