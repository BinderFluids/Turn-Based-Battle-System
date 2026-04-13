using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class SocketCursor : MonoBehaviour
{
    [Header("Manager")]
    [SerializeField] private SocketEditorManager m_socketEditorManager;
    private IEnumerable<SocketHandle> m_socketHandles => m_socketEditorManager.SocketHandles;
    
    [Header("Camera")]
    [SerializeField] private Camera m_camera;
    private SocketHandle m_currentDraggingSocket;
    private SocketHandle m_currentDetailedSocket;
    private SocketHandle m_targetSocket;
    private SocketHandle m_currentHoveredSocket;

    [Header("UI")]
    [SerializeField] private TMP_Text m_socketNameText;
    [SerializeField] private TMP_InputField m_newSocketDataNameField; 
    
    [Header("Input")]
    [SerializeField] private float m_range;
    [SerializeField] private float m_snapDistance;
    private KeyCode[] m_deleteKeys =
    {
        KeyCode.Delete,
        KeyCode.Backspace,
        KeyCode.X
    };
    private KeyCode[] m_newSocketKeys =
    {
        KeyCode.Space
    };

    public void Update()
    {
        _GetMouseWorldPos(out Vector3 mouseWorldPos);
        _DragSockets(mouseWorldPos);
        _ShowSocketDetails(mouseWorldPos);
        _HandleHover(mouseWorldPos);  
        
        //Tanner: Only handle keyboard input if no typing element is being interacted with
        if (!m_socketHandles.Any(handle => handle.b_isTyping) && !m_newSocketDataNameField.isFocused)
            _HandleInput(mouseWorldPos);
    }

    private void _HandleInput(Vector3 mouseWorldPos)
    {
        _HandleNewSocket(mouseWorldPos);
        _HandleDeleteSocket();
    }
    
    private void _HandleHover(Vector3 mouseWorldPos)
    {
        SocketHandle hoveredHandle = _GetClosestSocketHandle(m_socketHandles, mouseWorldPos);
        if (hoveredHandle == null)
        {   
            m_currentHoveredSocket?.Hover(false);
            m_currentHoveredSocket = null;
            return;
        }
        if (hoveredHandle != m_currentHoveredSocket)
        {
            m_currentHoveredSocket?.Hover(false);
            m_currentHoveredSocket = hoveredHandle;
            m_currentHoveredSocket.Hover(true); 
        }
    }

    private void _HandleDeleteSocket()
    {
        foreach (KeyCode key in m_deleteKeys)
        {
            if (Input.GetKeyDown(key) && m_targetSocket)
            {
                Destroy(m_targetSocket.gameObject);
                SetTargetSocket(null); 
                _ClearDetailedSocket();
                m_currentHoveredSocket = null; 
            }
        }
    }

    private void _HandleNewSocket(Vector3 mouseWorldPos)
    {
        foreach (KeyCode key in m_newSocketKeys)
            if (Input.GetKeyDown(key))
                m_socketEditorManager.CreateNewHandle(mouseWorldPos);
    }

    private void _GetMouseWorldPos(out Vector3 mouseWorldPos)
    {
        Vector3 mousePos = Mouse.current.position.ReadValue();
        mouseWorldPos = m_camera.ScreenToWorldPoint(mousePos);
        mouseWorldPos.z = 0;
    }
    
    private void _ShowSocketDetails(Vector3 mouseWorldPos)
    {
        if (Input.GetKeyDown(KeyCode.Mouse1))
            _ClearDetailedSocket();
        
        if (Input.GetKeyDown(KeyCode.F2))
            if (m_targetSocket != null)
                m_targetSocket.SetDetailsActive(true);
        
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            m_currentDetailedSocket = _GetClosestSocketHandle(m_socketHandles, mouseWorldPos);
            m_currentDetailedSocket?.SetDetailsActive(true);
            SetTargetSocket(m_currentDetailedSocket);        
        }
    }
    
    private void _DragSockets(Vector3 mouseWorldPos)
    {
        if (Input.GetKeyDown(KeyCode.Mouse0) && m_currentDraggingSocket == null)
        {
            m_currentDraggingSocket = _GetClosestSocketHandle(m_socketHandles, mouseWorldPos);
            SetTargetSocket(m_currentDraggingSocket);
            //_ClearDetailedSocket();
        }

        if (m_currentDraggingSocket != null)
        {
            m_currentDraggingSocket.transform.position = Input.GetKey(KeyCode.LeftShift)
                ? _SnapToGrid(mouseWorldPos)
                : mouseWorldPos;
        }

        if (Input.GetKeyUp(KeyCode.Mouse0))
            m_currentDraggingSocket = null;
    }

    private Vector3 _SnapToGrid(Vector3 position)
    {
        return new Vector3(
            Mathf.Round(position.x / m_snapDistance) * m_snapDistance, 
            Mathf.Round(position.y / m_snapDistance) * m_snapDistance, 
            position.z
            );
    }
    
    private void _ClearDetailedSocket()
    {
        if (m_currentDetailedSocket == null) return; 
        
        m_currentDetailedSocket?.SetDetailsActive(false);
        m_currentDetailedSocket = null;
    }

    public void SetTargetSocket(SocketHandle socketHandle)
    {
        if (socketHandle == m_targetSocket) return;
        
        m_targetSocket?.Deselect();
        m_targetSocket = socketHandle;
        m_targetSocket?.Select();
        
        m_socketNameText.text = socketHandle == null ? string.Empty : m_targetSocket.SocketName;
    }
    
    private SocketHandle _GetClosestSocketHandle(IEnumerable<SocketHandle> candidates, Vector3 origin)
    {
        SocketHandle closest = null;
        float minDistance = float.MaxValue;
        
        foreach (SocketHandle candidate in candidates)
        {
            if (candidate == null) continue;

            float distance = Vector3.Distance(origin, candidate.transform.position);
            if (distance > m_range) continue;
            
            if (distance < minDistance)
            {
                minDistance = distance;
                closest = candidate; 
            }
        }

        return closest; 
    }
}