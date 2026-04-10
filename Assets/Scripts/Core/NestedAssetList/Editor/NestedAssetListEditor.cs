using System;
using System.Linq;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

[CustomPropertyDrawer(typeof(NestedAssetList<>), true)]
public class NestedAssetListEditor : PropertyDrawer
{
    const string k_NestedAssetsPropertyName = "nestedAssets";
    const string k_ParentAssetPropertyName = "parentAsset";

    private ScriptableObject parentAsset;
    private Type childType;
    private Type[] concreteStepTypes;
    private ReorderableList reorderableList;
    private bool isInitialized;
    
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);

        SerializedProperty parentAssetProperty = property.FindPropertyRelative(k_ParentAssetPropertyName);

        EditorGUI.BeginChangeCheck();

        float line = EditorGUIUtility.singleLineHeight;
        float vSpace = EditorGUIUtility.standardVerticalSpacing;

        Rect rowRect = new Rect(position.x, position.y, position.width, line);
        rowRect.y += line + vSpace;

        Rect helpRect = new Rect(position.x, rowRect.y, position.width, line * 2f);

        // Check the *actual inspected object*, not the type that declared the field.
        if (property.serializedObject.targetObject is not ScriptableObject ownerSo)
        {
            EditorGUI.HelpBox(helpRect, "Nested Asset List must be used on a ScriptableObject", MessageType.Warning);

            if (EditorGUI.EndChangeCheck())
                property.serializedObject.ApplyModifiedProperties();

            EditorGUI.EndProperty();
            return;
        }

        parentAssetProperty.objectReferenceValue = ownerSo;

        // If you specifically require this to be an *asset on disk* (not just any ScriptableObject instance):
        if (string.IsNullOrEmpty(AssetDatabase.GetAssetPath(ownerSo)))
        {
            EditorGUI.HelpBox(helpRect, "Parent Asset must be saved as a ScriptableObject asset first", MessageType.Warning);

            if (EditorGUI.EndChangeCheck())
                property.serializedObject.ApplyModifiedProperties();

            EditorGUI.EndProperty();
            return;
        }

        RefreshTypes();
        RefreshTypes();
        if (reorderableList == null) 
        {
            ConfigureReordableList(property);
        }
        else 
        {
            // CRITICAL: Update serializedProperty reference every frame
            reorderableList.serializedProperty = property.FindPropertyRelative(k_NestedAssetsPropertyName);
        }

        float listHeight = reorderableList.GetHeight();
        Rect listRect = new Rect(position.x, rowRect.y, position.width, listHeight);
        reorderableList.DoList(listRect);

        if (EditorGUI.EndChangeCheck())
            property.serializedObject.ApplyModifiedProperties();

        EditorGUI.EndProperty();
    }
    
    void RefreshTypes()
    {
        childType = fieldInfo.FieldType.GetGenericArguments()[0];
        if (concreteStepTypes == null)
        {
            concreteStepTypes = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(a =>
                {
                    try { return a.GetTypes(); }
                    catch { return Type.EmptyTypes; }
                })
                .Where(t => t != childType && childType.IsAssignableFrom(t) && !t.IsAbstract)
                .ToArray();
        }
    }
    
    void ConfigureReordableList(SerializedProperty property)
    {
        SerializedProperty nestedAssetsProperty = property.FindPropertyRelative(k_NestedAssetsPropertyName);
        SerializedProperty parentAssetProperty = property.FindPropertyRelative(k_ParentAssetPropertyName);
        
        if  (parentAssetProperty.objectReferenceValue is not ScriptableObject detectedParentAsset)
            throw new ArgumentNullException(nameof(parentAsset) + " must be a ScriptableObject");
        if (detectedParentAsset == null) throw new NullReferenceException(); 
        parentAsset = detectedParentAsset;
        
        reorderableList = new ReorderableList(
            property.serializedObject, 
            nestedAssetsProperty, 
            true, 
            true, 
            true, 
            false
        );
        
        //Tanner: Display header
        reorderableList.drawHeaderCallback = rect =>
            EditorGUI.LabelField(rect, $"Nested {childType.Name}");

        //Tanner: Display each item
        reorderableList.drawElementCallback = (rect, index, isActive, isFocused) =>
        {
            SerializedProperty element = reorderableList.serializedProperty.GetArrayElementAtIndex(index);
            ScriptableObject obj = element.objectReferenceValue as ScriptableObject;
        
            Rect r1 = new Rect(rect.x, rect.y, rect.width * 0.6f, EditorGUIUtility.singleLineHeight);
            Rect r2 = new Rect(rect.x + rect.width * 0.62f, rect.y, rect.width * 0.18f, EditorGUIUtility.singleLineHeight);
            Rect r3 = new Rect(rect.x + rect.width * 0.82f, rect.y, rect.width * 0.18f, EditorGUIUtility.singleLineHeight);

            string displayString; 
            if (obj == null) displayString = "(Missing)";
            else if (obj is INestableAsset nestable) displayString = nestable.GetListDisplayName(); 
            else displayString = obj.name;
            GUI.Label(r1, displayString);
            
            if (GUI.Button(r2, "Edit"))
                if (obj) Selection.activeObject = obj;
            
            if (GUI.Button(r3, "Delete"))
                RemoveAssetAt(index, property);
        };

        
        //Tanner: Dropdown for adding a new asset
        reorderableList.elementHeight = EditorGUIUtility.singleLineHeight + 6;
        reorderableList.onAddDropdownCallback = (rect, list) =>
        {
            GenericMenu menu = new GenericMenu();
            foreach (Type t in concreteStepTypes)
            {
                string name = t.Name;
                menu.AddItem(new GUIContent(name), false, () => AddAssetOfType(t, property));
            }
            
            //Tanner: Display unclickable item if no attack types exist
            if (concreteStepTypes.Length == 0)
                menu.AddItem(new GUIContent($"No {childType.Name}s found"), false, null);

            menu.ShowAsContext();
        };
        
        //Tanner: Update the list order in the actual serialized object/list
        reorderableList.onReorderCallbackWithDetails = (reorderedList, oldIndex, newIndex) =>
            ReorderAssets(oldIndex, newIndex, property);

        isInitialized = true;
    }
    void AddAssetOfType(Type childAssetType, SerializedProperty property)
    {
        string path = AssetDatabase.GetAssetPath(parentAsset);
        if (string.IsNullOrEmpty(path))
        {
            Debug.LogError("Nested Asset Parent must be saved as an Scriptable Object First");
            return;
        }
        property.serializedObject.Update();
        
        ScriptableObject instance = ScriptableObject.CreateInstance(childAssetType);
        instance.name = childAssetType.Name;

        //Tanner: Nest new asset under the parent scriptable object
        Undo.RegisterCompleteObjectUndo(parentAsset, $"Add {childType.Name}");
        AssetDatabase.AddObjectToAsset(instance, parentAsset);
        
        SerializedProperty nestedAssets = property.FindPropertyRelative(k_NestedAssetsPropertyName);
        nestedAssets.arraySize++;
        nestedAssets.GetArrayElementAtIndex(nestedAssets.arraySize - 1).objectReferenceValue = instance;
        
        property.serializedObject.ApplyModifiedProperties();
        
        EditorUtility.SetDirty(instance);
        EditorUtility.SetDirty(parentAsset);
        AssetDatabase.SaveAssets();
        AssetDatabase.ImportAsset(path);

        //Tanner: Open newly created object in inspector
        Selection.activeObject = instance;
    }

    void RemoveAssetAt(int index, SerializedProperty property)
    {
        SerializedProperty nestedAssets = property.FindPropertyRelative(k_NestedAssetsPropertyName);

        if (index < 0 || index >= nestedAssets.arraySize)
            return;

        ScriptableObject childAsset = nestedAssets.GetArrayElementAtIndex(index).objectReferenceValue as ScriptableObject;

        Undo.RegisterCompleteObjectUndo(parentAsset, "Remove Nested Asset");

        // Remove the sub-asset from the asset file.
        if (childAsset != null)
        {
            AssetDatabase.RemoveObjectFromAsset(childAsset);
            Undo.DestroyObjectImmediate(childAsset);
        }

        // Remove the reference from the array.
        nestedAssets.DeleteArrayElementAtIndex(index);

        // Object-reference arrays usually need a second delete to remove the slot fully.
        if (index < nestedAssets.arraySize && nestedAssets.GetArrayElementAtIndex(index).objectReferenceValue == null)
        {
            nestedAssets.DeleteArrayElementAtIndex(index);
        }

        property.serializedObject.ApplyModifiedProperties();

        EditorUtility.SetDirty(parentAsset);
        AssetDatabase.SaveAssets();
        AssetDatabase.ImportAsset(AssetDatabase.GetAssetPath(parentAsset));
    }
    
    //Tanner: idk i found this online
    void ReorderAssets(int oldIndex, int newIndex, SerializedProperty property)
    {
        SerializedProperty nestedAssets = property.FindPropertyRelative(k_NestedAssetsPropertyName);
        
        if (oldIndex == newIndex) return;
        ScriptableObject element = nestedAssets.GetArrayElementAtIndex(oldIndex).objectReferenceValue as ScriptableObject;
        nestedAssets.DeleteArrayElementAtIndex(oldIndex);
        
        nestedAssets.InsertArrayElementAtIndex(newIndex);
        nestedAssets.GetArrayElementAtIndex(newIndex).objectReferenceValue = element;

        EditorUtility.SetDirty(parentAsset);
        AssetDatabase.SaveAssets();
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        float line = EditorGUIUtility.singleLineHeight;
        float vSpace = EditorGUIUtility.standardVerticalSpacing;

        SerializedProperty parentAssetProperty = property.FindPropertyRelative(k_ParentAssetPropertyName);
        parentAssetProperty.objectReferenceValue = property.serializedObject.targetObject as ScriptableObject;
        
        if (parentAssetProperty.objectReferenceValue == null)
            return line + vSpace + (line * 2f);

        if (!isInitialized || reorderableList == null)
            return line + vSpace + (line * 3f);

        return line + vSpace + reorderableList.GetHeight();
    }
}
