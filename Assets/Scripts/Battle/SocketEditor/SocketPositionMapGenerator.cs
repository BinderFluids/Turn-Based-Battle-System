using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Battle.SocketEditor
{
    public class SocketPositionMapGenerator : MonoBehaviour
    {
        [SerializeField] private SocketPositionMap activeData; 
        private readonly string SOCKET_DATA_PATH = "Assets/Data/SocketPositionMaps";
        private readonly string DEFAULT_SOCKET_NAME = "DefaultSocket";
        
        public SocketPositionMap Generate(string dataName, IEnumerable<SocketHandle> handles)
        {
            if (!Directory.Exists(SOCKET_DATA_PATH))
                Directory.CreateDirectory(SOCKET_DATA_PATH);
            
            //Tanner: error checking so you can't save faulty information
            foreach (SocketHandle handle in handles)
            {
                if (handle.name == string.Empty)
                {
                    Debug.LogError("No socket name can be empty!");
                    return null;
                }
                if (handles.Count(h => h.name == handle.name) > 1)
                {
                    Debug.LogError($"Duplicate socket name '{handle.name}'!");
                    return null;
                }
            }
            
            string newName = dataName;
            string pathName = newName == string.Empty ? DEFAULT_SOCKET_NAME : newName;
            string path = Path.Combine(SOCKET_DATA_PATH, $"{pathName}.asset");

            if (!File.Exists(path))
            {
                Debug.Log("Creating new asset at " + path + "");
                SocketPositionMap newData = ScriptableObject.CreateInstance<SocketPositionMap>();
                AssetDatabase.CreateAsset(newData, path);
                AssetDatabase.SaveAssets();
                
                activeData = newData;
            }
            else
                activeData = AssetDatabase.LoadAssetAtPath<SocketPositionMap>(path);
            
            
            EditorUtility.SetDirty(activeData); 
            
            activeData.Clear();
            foreach (var handle in handles)
                activeData.AddSocket(handle.name, handle.transform.position);
            
            //Tanner: Save the asset
            AssetDatabase.SaveAssets(); 
            AssetDatabase.Refresh();
            
            //Tanner: Print out save details
            Debug.Log("--- Save Socket Data! ---");
            foreach (SocketHandle handle in handles)
                Debug.Log($"Socket: {handle.name} Position: {handle.transform.position}");
            Debug.Log("--- End Save Socket Data! ---");
            
            return activeData;
        }


    }
}