using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace Battle.Editor
{
    [CustomEditor(typeof(BattleEntity))]
    [CanEditMultipleObjects]
    public class BattleEntityEditor : UnityEditor.Editor
    {
        private int index;
        private BattleEntity entity; 
        
        public override void OnInspectorGUI()
        {
            entity = (BattleEntity)target;
            
            base.OnInspectorGUI();
            if (serializedObject.isEditingMultipleObjects) return;
            
            DrawStartTurnButton(); 
            DrawComponentMenu();
        }

        void DrawStartTurnButton()
        {
            if (!Application.isPlaying) return;
        
            if (GUILayout.Button("Start Turn"))
            {
                // if (entity.TryGetComponent(out TurnComponent turnComponent))
                //     turnComponent.StartTurn();
                //TODO TRIGGER TURN START EVENT
                
            }
        }

        void DrawComponentMenu()
        {
            
            
            var componentTypes = RetrieveAllComponentTypes().OrderBy(t => t.Name).ToList(); 
            List<string> options = componentTypes.Select(t => t.Name).ToList();
            options.Insert(0, "(select)");
            
            EditorGUI.BeginChangeCheck();
            index = EditorGUILayout.Popup("Add Component", index, options.ToArray());

            if (EditorGUI.EndChangeCheck())
            {
                if (index == 0) return;
                Type selectedType = componentTypes.ElementAt(index-1);

                
                index = 0; 
                if (entity.TryGetComponent(selectedType, out var component))
                    return;
                
                var newComponent = entity.gameObject.AddComponent(selectedType) as BattleEntityComponent;
                var entityFieldInfo = typeof(BattleEntityComponent).GetField("entity", BindingFlags.NonPublic | BindingFlags.Instance);
                entityFieldInfo.SetValue(newComponent, entity);
            }
        }

        List<Type> RetrieveAllComponentTypes()
        {
            return AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(a => a.GetTypes())
                .Where(t => t.IsSubclassOf(typeof(BattleEntityComponent))).ToList(); 
        }
    }
}