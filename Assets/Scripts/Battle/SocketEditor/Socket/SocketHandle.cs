using System;
using TMPro;
using UnityEngine;

public class SocketHandle : MonoBehaviour
{
    public string SocketName => name;
    public event Action<SocketHandle> onDestroy;
    [SerializeField] private Canvas m_canvas; 
    [SerializeField] private GameObject m_detailsContainer;
    [SerializeField] private GameObject m_hoverName;
    [SerializeField] private GameObject m_selectedHighlight; 
    [SerializeField] private TMP_InputField m_socketNameField;
    [SerializeField] private SpriteRenderer m_socketSpriteRenderer;
    public bool b_isTyping => m_socketNameField.isFocused;

    public override string ToString()
    {
        return $"{SocketName}: {transform.position}";
    }
    
    public void Init(string socketName, Vector2 position)
    {
        name = socketName;
        transform.position = position;
        m_socketNameField.text = socketName;
        m_canvas.worldCamera = Camera.main;
    }

    public void Select()
    {
        m_selectedHighlight.SetActive(true); 
    }

    public void Deselect()
    {
        m_selectedHighlight.SetActive(false);
    }
    
    public void SetDetailsActive(bool active)
    {
        m_detailsContainer.SetActive(active);
    }

    private void OnDestroy()
    {
        onDestroy?.Invoke(this);
    }

    public void Hover(bool active)
    {
        m_hoverName.SetActive(active);
    }
}
