using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace Battle.Window.Editor
{
    [CustomPropertyDrawer(typeof(ActionCommandTierGradient))]
    public class ActionCommandTierGradientDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            Event guiEvent = Event.current;
            ActionCommandTierGradient gradient = fieldInfo.GetValue(property.serializedObject.targetObject) as ActionCommandTierGradient;
            if (gradient == null) return;
            
            Rect labelRect = new Rect(position); 
            labelRect.width = EditorGUIUtility.labelWidth;
            
            Rect textureRect = new Rect(position);
            textureRect.x += EditorGUIUtility.labelWidth;
            textureRect.width -= EditorGUIUtility.labelWidth;

            if (guiEvent.type == EventType.Repaint)
            {
                GUI.DrawTexture(textureRect, gradient.GetTexture());
                GUI.Label(labelRect, label.text); 
            }
            else if (guiEvent.type == EventType.MouseDown && guiEvent.button == 0 &&
                     textureRect.Contains(guiEvent.mousePosition))
            {
                ActionCommandTierGradientWindow window = EditorWindow.GetWindow<ActionCommandTierGradientWindow>();
                window.SetGradient(gradient);
            }
        }
    }
}