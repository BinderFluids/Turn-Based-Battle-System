using System;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using Object = UnityEngine.Object;

    [CustomEditor(typeof(NestedAssetParent), true)]
    public class NestedAssetParentEditor : UnityEditor.Editor
    {
        private ReorderableList _reorderableList;
        private SerializedProperty _childs;
        private Type _childType;

        private void OnEnable()
        {
            // For getting the Type value
            try
            {
                _childType = (target as NestedAssetParent).nestedAssetChildType;
                if (!_childType.IsSubclassOf(typeof(NestedAsset))) throw new ArgumentException();
            }
            catch (ArgumentException argument)
            {
                Debug.LogError($"Child Type - {_childType} is not a subclass of NestedAsset. {argument}");
            }
            catch (Exception e)
            {
                Debug.LogError($"Error getting child type of NestedAsset {e}");
            }
            
            
            _childs = serializedObject.FindProperty(NestedAssetParent.childs_prop_name);
            
            _reorderableList = new ReorderableList(serializedObject, _childs);
            _reorderableList.onAddCallback = OnAddCallback;
            _reorderableList.onRemoveCallback = OnRemoveCallback;
            _reorderableList.drawElementCallback = DrawElementCallback;
        }

        private void DrawElementCallback(Rect rect, int index, bool isactive, bool isfocused)
        {
            SerializedObject child = new SerializedObject(_childs.GetArrayElementAtIndex(index).objectReferenceValue);
            var childName = child.FindProperty(NestedAsset.editor_name_prop_name);
        
            child.Update();

            rect.height = EditorGUIUtility.singleLineHeight;
            rect.y += 2f;
            EditorGUI.PropertyField(rect, childName, new GUIContent("Child " + index));

            if (child.ApplyModifiedProperties())
            {
                if (child.targetObject.name != childName.stringValue)
                {
                    child.targetObject.name = childName.stringValue;
                    AssetDatabase.SaveAssets();
                }
            }
        }

        private void OnRemoveCallback(ReorderableList list)
        {
            DeleteChild(list.index);
        }

        private void OnAddCallback(ReorderableList list)
        {
            _childs.arraySize++;
            _childs.GetArrayElementAtIndex(_childs.arraySize - 1).objectReferenceValue = CreateChild();
        }

        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            GUILayout.Space(10);
            
            serializedObject.Update();
        
            _reorderableList.DoLayoutList();

            serializedObject.ApplyModifiedProperties();
        }

        private NestedAsset CreateChild()
        {
            // Create an instance of the specific type T instead of base NestedAsset
            var child = CreateInstance(_childType) as NestedAsset;
            if (child != null)
            {
                child.name = _childType.Name;
                AssetDatabase.AddObjectToAsset(child, target);
                AssetDatabase.SaveAssets();
            }
            return child;

        }

        private void DeleteChild(int index)
        {
            var child = _childs.GetArrayElementAtIndex(index).objectReferenceValue;
            _childs.GetArrayElementAtIndex(index).objectReferenceValue = null;
            _childs.DeleteArrayElementAtIndex(index);
            AssetDatabase.RemoveObjectFromAsset(child);
            DestroyImmediate(child, true);
            AssetDatabase.SaveAssets();
        }
    }
