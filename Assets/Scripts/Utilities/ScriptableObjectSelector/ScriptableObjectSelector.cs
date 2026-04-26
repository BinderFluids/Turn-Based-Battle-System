using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

public class ScriptableObjectSelector<T> : MonoBehaviour where T : ScriptableObject
{
    public T selectedObject => m_selectedObject;
    [SerializeField] private TMP_Dropdown m_dropdown;

    public UnityEvent<T> onValueChanged;
    
    [SerializeField] private List<T> m_scriptableObjects = new();
    private T m_selectedObject;

    private void Awake()
    {
        _GetAllInstances();
        _PopulateDropdown();
    }

    public void Refresh()
    {
        Refresh(selectedObject);
    }
    public void Refresh(T selectedObject)
    {
        _GetAllInstances();
        
        int index = m_scriptableObjects.IndexOf(selectedObject);
        _PopulateDropdown(index);
    }
    
    private void _GetAllInstances()
    {
        //Tanner: Find all instances of the scriptable object
        m_scriptableObjects.Clear();
        string[] guids = AssetDatabase.FindAssets("t:" + typeof(T).Name);
        foreach (string guid in guids)
            m_scriptableObjects.Add(_LoadFromGUID(guid));
    }

    private void _PopulateDropdown(int index = 0)
    {
        m_dropdown.ClearOptions();
        m_dropdown.AddOptions(m_scriptableObjects.Select(obj => obj.name).ToList());
        m_dropdown.onValueChanged.AddListener(_DropDownEventListener);
        _DropDownEventListener(index); 
    }

    void _DropDownEventListener(int index)
    {
        if (index > m_scriptableObjects.Count - 1) return; 
        
        m_selectedObject = m_scriptableObjects[index];
        onValueChanged.Invoke(m_selectedObject);
    }
    
    private T _LoadFromGUID(string guid)
    {
        T newObject = AssetDatabase.LoadAssetAtPath<T>(AssetDatabase.GUIDToAssetPath(guid));
        return newObject;
    }
}