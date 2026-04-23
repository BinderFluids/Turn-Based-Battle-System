using UnityEditor;
using UnityEngine;

namespace Battle.Editor
{
    [CustomEditor(typeof(BattleEntity))]
    [CanEditMultipleObjects]
    public class BattleEntityEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
        
            if (serializedObject.isEditingMultipleObjects) return;
            if (!Application.isPlaying) return;
        
            if (GUILayout.Button("Start Turn"))
            {
                BattleEntity entity = target as BattleEntity;
                // if (entity.TryGetComponent(out TurnComponent turnComponent))
                //     turnComponent.StartTurn();
                //TODO TRIGGER TURN START EVENT
                
            }
        }
    }
}