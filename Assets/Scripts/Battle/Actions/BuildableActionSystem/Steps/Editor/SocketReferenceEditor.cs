using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(SocketReference))]
public class SocketReferenceEditor :  PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        SerializedProperty socketDataProperty = property.FindPropertyRelative("m_socketData");
        
        //Tanner: Error handling
        if (socketDataProperty.objectReferenceValue == null)
        {
            ThrowError(property, position, "No socket data assigned.");
            return;
        }
        SocketData socketData = socketDataProperty.objectReferenceValue as SocketData; 
        if (socketData == null)
        {
            ThrowError(property, position, "Socket data is null.");
            return;
        }
        List<string> socketNames = socketData.GetSocketPositionsDictionary().Keys.ToList();
        if (socketNames.Count == 0)
        {
            ThrowError(property, position, "Socket data has no items.");
            return;
        }
        if (socketNames.Contains(null))
        {
            ThrowError(property, position, "Socket contains null socket name.");
            return;
        }
        
        //Tanner: Dropdown logic
        SerializedProperty selectedSocketNameProperty = property.FindPropertyRelative("m_selectedSocketName");
        string selectedSocketName = selectedSocketNameProperty.stringValue;
    
        int chosenIndex = 0;
        if (selectedSocketName != string.Empty)
            chosenIndex = Mathf.Max(0, socketNames.IndexOf(selectedSocketName));
        
        chosenIndex = EditorGUI.Popup(position, label.text, chosenIndex, socketNames.ToArray());
        selectedSocketNameProperty.stringValue = socketNames[chosenIndex];
    }

    void ThrowError(SerializedProperty property, Rect position, string message)
    {
        EditorGUI.HelpBox(position, message, MessageType.Error);
        if (GUILayout.Button("Refresh Socket Data"))
        {
            property.FindPropertyRelative("m_hiddenValidateValue").intValue++;
            EditorUtility.SetDirty(property.serializedObject.targetObject);
        }
    }
}