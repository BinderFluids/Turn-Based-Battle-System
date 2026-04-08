using System;
using System.Linq;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

[CustomEditor(typeof(NestedAssetParent), editorForChildClasses:true)]
public class NestedAssetParentEditor : Editor
{
    private static Type[] m_ConcreteStepTypes;
    private ReorderableList m_reordableList;
    private NestedAssetParent parent;
    private Type childType => parent.nestedAssetChildType;
    
    void OnEnable()
    {
        parent = (NestedAssetParent)target;
        
        _RefreshTypes();
        _ConfigureReordableList();
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        base.OnInspectorGUI();
        EditorGUILayout.Space();
        m_reordableList.DoLayoutList();
        serializedObject.ApplyModifiedProperties();
    }
    
    //Tanner: Finds all the types of AttackSteps and adds them to the array
    void _RefreshTypes()
    {
        if (m_ConcreteStepTypes == null)
        {
            m_ConcreteStepTypes = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(a =>
                {
                    try { return a.GetTypes(); }
                    catch { return new Type[0]; }
                })
                .Where(t => t != childType && childType.IsAssignableFrom(t) && !t.IsAbstract)
                .ToArray();
        }
    }

    void _ConfigureReordableList()
    {
        //Tanner: Create the reorderable list
        m_reordableList = new ReorderableList( serializedObject, serializedObject.FindProperty("nestedChildren")); 
        
        //Tanner: Display header
        m_reordableList.drawHeaderCallback = rect =>
            EditorGUI.LabelField(rect, $"Nested {childType.Name}");

        //Tanner: Display each item
        m_reordableList.drawElementCallback = (rect, index, isActive, isFocused) =>
        {
            SerializedProperty element = m_reordableList.serializedProperty.GetArrayElementAtIndex(index);
            NestedAsset obj = element.objectReferenceValue as NestedAsset;

            Rect r1 = new Rect(rect.x, rect.y, rect.width * 0.6f, EditorGUIUtility.singleLineHeight);
            Rect r2 = new Rect(rect.x + rect.width * 0.62f, rect.y, rect.width * 0.18f, EditorGUIUtility.singleLineHeight);
            Rect r3 = new Rect(rect.x + rect.width * 0.82f, rect.y, rect.width * 0.18f, EditorGUIUtility.singleLineHeight);
            
            GUI.Label(r1, obj == null ? "(Missing)" : obj.GetNestedAssetName());
            
            if (GUI.Button(r2, "Edit"))
                if (obj) Selection.activeObject = obj;
            
            if (GUI.Button(r3, "Delete"))
                _RemoveStepAt(index);
        };

        
        //Tanner: Dropdown for adding a new Attack Step
        m_reordableList.elementHeight = EditorGUIUtility.singleLineHeight + 6;
        m_reordableList.onAddDropdownCallback = (rect, list) =>
        {
            GenericMenu menu = new GenericMenu();
            foreach (Type t in m_ConcreteStepTypes)
            {
                string name = t.Name;
                menu.AddItem(new GUIContent(name), false, () => _AddStepOfType(t));
            }
            
            //Tanner: Display unclickable item if no attack types exist
            if (m_ConcreteStepTypes.Length == 0)
                menu.AddItem(new GUIContent($"No {childType.Name}s found"), false, null);

            menu.ShowAsContext();
        };
        
        //Tanner: Update the list order in the actual serialized object/list
        m_reordableList.onReorderCallbackWithDetails = (reorderedList, oldIndex, newIndex) =>
            ReorderAssets(oldIndex, newIndex);
    }

    void _AddStepOfType(Type stepType)
    {
        string path = AssetDatabase.GetAssetPath(parent);
        if (string.IsNullOrEmpty(path))
        {
            Debug.LogError("Nested Asset Parent must be saved as an Scriptable Object First");
            return;
        }
        
        NestedAsset instance = (NestedAsset)CreateInstance(stepType);
        instance.name = instance.GetNestedAssetName();

        //Tanner: Nest new attack step under the attack scriptable object
        Undo.RegisterCompleteObjectUndo(parent, $"Add {childType.Name}");
        AssetDatabase.AddObjectToAsset(instance, parent);
        AssetDatabase.SaveAssets();
        AssetDatabase.ImportAsset(path);
        
        parent.NestedChildren.Add(instance);
        EditorUtility.SetDirty(instance);
        EditorUtility.SetDirty(parent);

        //Tanner: Open newly created object in inspector
        Selection.activeObject = instance;
    }

    void _RemoveStepAt(int index)
    {
        if (index < 0 || index >= parent.NestedChildren.Count) return;

        NestedAsset step = parent.NestedChildren[index];
        
        //Tanner: Less cleanup on null items
        if (step == null)
        {
            parent.NestedChildren.RemoveAt(index);
            EditorUtility.SetDirty(parent);
            AssetDatabase.SaveAssets();
            return;
        }

        //Tanner: Register AttackStep on undo stack
        Undo.RegisterCompleteObjectUndo(parent, "Remove Nested Asset");
        parent.NestedChildren.RemoveAt(index);
        Undo.DestroyObjectImmediate(step);

        EditorUtility.SetDirty(parent);
        AssetDatabase.SaveAssets();
    }
    
    //Tanner: idk i found this online
    void ReorderAssets(int oldIndex, int newIndex)
    {
        if (oldIndex == newIndex) return;
        NestedAsset element = parent.NestedChildren[oldIndex];
        parent.NestedChildren.RemoveAt(oldIndex);
        parent.NestedChildren.Insert(newIndex, element);
        EditorUtility.SetDirty(parent);
        AssetDatabase.SaveAssets();
    }
}
