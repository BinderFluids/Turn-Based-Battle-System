using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ScriptableObjectAssetRetriever<T> where T : ScriptableObject
{
    
    
    public List<T> GetAllInstances()
    {
        List<T> scriptableObjects = new List<T>();
        
        //Tanner: Find all instances of the scriptable object
        string[] guids = AssetDatabase.FindAssets("t:" + typeof(T).Name);
        foreach (string guid in guids)
            scriptableObjects.Add(LoadFromGUID(guid));

        return scriptableObjects;
    }
    
    private T LoadFromGUID(string guid)
    {
        T newObject = AssetDatabase.LoadAssetAtPath<T>(AssetDatabase.GUIDToAssetPath(guid));
        return newObject;
    }
}