using System.IO; 
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System.Linq;
using TMPro;
using UnityEngine.Events;
using UnityEngine.UI;

public class SocketEditorManager : MonoBehaviour
{
    [SerializeField] private SocketHandle m_socketHandlePrefab;

    private const string m_kSocketDataPath = "Assets/Resources/SocketData/";
    private const string m_kDefaultNewSocketName = "New Socket";
    [SerializeField] private SocketData m_activeData;
    private List<SocketHandle> m_socketHandlesList = new();
    public IReadOnlyList<SocketHandle> SocketHandles => m_socketHandlesList;

    [SerializeField] private SocketCursor m_socketCursor;
    [SerializeField] private SocketDataSelector m_socketDataSelector;
    
    [Header("Events")] 
    public UnityEvent onSave;
    public UnityEvent onClear;
    public UnityEvent<SocketData> onNewSocketData;
    public UnityEvent onLoad;

    [Header("UI")] 
    [SerializeField] private GameObject m_hideableContainer; 
    [SerializeField] private TMP_InputField m_newSocketNameField;
    [SerializeField] private Button m_newSocketButton;
    [SerializeField] private Button m_saveButton;
    [SerializeField] private Button m_loadButton;
    [SerializeField] private Button m_clearButton;

    private void Update()
    {
        _SetButtonInteractionStates();
        _HandleInput();
    }

    private void _HandleInput()
    {
        if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.S))
            SaveData();
        if (Input.GetKeyDown(KeyCode.Tab))
            m_hideableContainer.SetActive(!m_hideableContainer.activeSelf);
    }
    
    private void _SetButtonInteractionStates()
    {
        //Tanner: Enable/disable buttons
        m_clearButton.interactable = m_socketHandlesList.Count > 0;
    }
    
    public void CreateNewHandle(string name, Vector3 position, bool focusCursor = true)
    {
        SocketHandle newHandle = Instantiate(m_socketHandlePrefab, Vector3.zero, Quaternion.identity);
        newHandle.onDestroy += _RemoveHandleFromList; 
        newHandle.Init(name, position);
        
        m_socketHandlesList.Add(newHandle);
        if (focusCursor) m_socketCursor.SetTargetSocket(newHandle);
    }
    public void CreateNewHandle(string name)
    {
        CreateNewHandle(name, Vector3.zero);
    }
    public void CreateNewHandle()
    {
        CreateNewHandle(m_socketHandlesList.Count.ToString());
    }
    public void CreateNewHandle(Vector3 position)
    {
        CreateNewHandle(m_socketHandlesList.Count.ToString(), position);
    }
    private void _RemoveHandleFromList(SocketHandle handle)
    {
        m_socketHandlesList.Remove(handle);
        
    }
    
    public void LoadData()
    {
        if (m_activeData == null)
        {
            Debug.LogError("No active data");
            return;
        }
        
        Clear();
        
        foreach (var kvp in m_activeData.GetSocketPositionsDictionary())
            CreateNewHandle(kvp.Key, kvp.Value, false);
        
        onLoad?.Invoke();
    }
    public void LoadData(SocketData data)
    {
        m_activeData = data;
        LoadData();
    }
    
    //Tanner: Forces the creation of a new asset if the names don't match
    public void SaveAs()
    {
        if (m_activeData != null)
            if (m_activeData.name != m_newSocketNameField.text)
                m_activeData = null; 
        SaveData();
    }
    public void SaveData()
    {
        if (!Directory.Exists(m_kSocketDataPath))
            Directory.CreateDirectory(m_kSocketDataPath);
        
        //Tanner: error checking so you can't save faulty information
        foreach (SocketHandle handle in m_socketHandlesList)
        {
            if (handle.SocketName == string.Empty)
            {
                Debug.LogError("No socket name can be empty!");
                return;
            }
            if (m_socketHandlesList.Count(h => h.SocketName == handle.SocketName) > 1)
            {
                Debug.LogError($"Duplicate socket name '{handle.SocketName}'!");
                return;
            }
        }
        
        //Tanner: Create new SO asset if one does not exist
        bool createdNewSocketAsset = false; 
        if (m_activeData == null)
        {
            string newName = m_newSocketNameField.text;
            string pathName = newName == string.Empty ? m_kDefaultNewSocketName : newName;
            string path = Path.Combine(m_kSocketDataPath, $"{pathName}.asset");

            if (File.Exists(path))
            {
                Debug.LogError($"Socket data asset already exists at {path}");
                return;
            }
            
            Debug.Log("Creating new asset at " + path + "");
            SocketData newData = ScriptableObject.CreateInstance<SocketData>();
            AssetDatabase.CreateAsset(newData, path);
            AssetDatabase.SaveAssets();
            
            m_activeData = newData;
            createdNewSocketAsset = true;
        }
        
        
        EditorUtility.SetDirty(m_activeData); 
        
        m_activeData.Clear();
        foreach (var handle in m_socketHandlesList)
            m_activeData.AddSocket(handle.SocketName, handle.transform.position);

        
        //Tanner: Save the asset
        AssetDatabase.SaveAssets(); 
        AssetDatabase.Refresh();
        
        //Tanner: Print out save details
        Debug.Log("--- Save Socket Data! ---");
        foreach (SocketHandle handle in m_socketHandlesList)
            Debug.Log($"Socket: {handle.SocketName} Position: {handle.transform.position}");
        Debug.Log("--- End Save Socket Data! ---");
        
        if (createdNewSocketAsset) 
            onNewSocketData?.Invoke(m_activeData);
        onSave?.Invoke();
    }

    public void Clear()
    {
        for (int i = m_socketHandlesList.Count - 1; i >= 0; i--)
            DestroyImmediate(m_socketHandlesList[i].gameObject);
        m_socketHandlesList.Clear();
        
        SocketHandle[] handles = FindObjectsOfType<SocketHandle>();
        foreach (SocketHandle handle in handles)
            DestroyImmediate(handle.gameObject);
        
        onClear?.Invoke();
    }
}
