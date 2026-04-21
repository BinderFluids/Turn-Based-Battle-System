using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(SocketReference))]
public class SocketReferenceEditor : PropertyDrawer
{
    private const float Spacing = 4f;
    private const float LeftPortion = 0.30f;
    private const float RightPortion = 0.70f;
    private const float MinObjectFieldWidth = 120f;
    private const float MinPopupWidth = 80f;

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        SerializedProperty socketDataProperty = property.FindPropertyRelative("socketPositionMap");
        SerializedProperty selectedSocketNameProperty = property.FindPropertyRelative("selectedSocketName");

        EditorGUI.BeginProperty(position, label, property);

        Rect contentRect = EditorGUI.PrefixLabel(position, label);
        float lineHeight = EditorGUIUtility.singleLineHeight;

        SocketPositionMap socketPositionMap = socketDataProperty.objectReferenceValue as SocketPositionMap;
        bool hasValidSocketData = socketPositionMap != null;

        // Full width until valid
        if (!hasValidSocketData)
        {
            socketDataProperty.objectReferenceValue = EditorGUI.ObjectField(
                contentRect,
                socketDataProperty.objectReferenceValue,
                typeof(SocketPositionMap),
                false
            );

            EditorGUI.EndProperty();
            return;
        }

        List<string> socketNames = socketPositionMap.GetSocketPositionsDictionary().Keys.ToList();

        if (socketNames.Count == 0)
        {
            DrawError(contentRect, "Socket data has no items.");
            EditorGUI.EndProperty();
            return;
        }

        if (socketNames.Contains(null))
        {
            DrawError(contentRect, "Socket contains null socket name.");
            EditorGUI.EndProperty();
            return;
        }

        float totalWidth = contentRect.width - Spacing;
        float objectWidth = Mathf.Max(MinObjectFieldWidth, totalWidth * LeftPortion);
        float popupWidth = Mathf.Max(MinPopupWidth, totalWidth * RightPortion);

        // Clamp if overflow
        float usedWidth = objectWidth + popupWidth + Spacing;
        if (usedWidth > contentRect.width)
        {
            float scale = contentRect.width / usedWidth;
            objectWidth *= scale;
            popupWidth *= scale;
        }

        Rect objectRect = new Rect(contentRect.x, contentRect.y, objectWidth, lineHeight);
        Rect popupRect = new Rect(objectRect.xMax + Spacing, contentRect.y, popupWidth, lineHeight);

        // Left: SocketData field
        socketDataProperty.objectReferenceValue = EditorGUI.ObjectField(
            objectRect,
            socketDataProperty.objectReferenceValue,
            typeof(SocketPositionMap),
            false
        );

        // Right: dropdown
        string selectedSocketName = selectedSocketNameProperty.stringValue;

        int chosenIndex = 0;
        if (!string.IsNullOrEmpty(selectedSocketName))
        {
            int index = socketNames.IndexOf(selectedSocketName);
            if (index >= 0)
                chosenIndex = index;
        }

        int newIndex = EditorGUI.Popup(popupRect, chosenIndex, socketNames.ToArray());
        if (newIndex >= 0 && newIndex < socketNames.Count)
        {
            selectedSocketNameProperty.stringValue = socketNames[newIndex];
        }

        EditorGUI.EndProperty();
    }

    private void DrawError(Rect position, string message)
    {
        EditorGUI.HelpBox(position, message, MessageType.Error);
    }
}