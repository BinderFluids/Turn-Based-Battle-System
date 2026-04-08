
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(BattleEntity))]
[CanEditMultipleObjects]
public class BattleEntityEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        
        if (serializedObject.isEditingMultipleObjects) return;
        if (!Application.isPlaying) return;
        
        if (GUILayout.Button("Start Turn"))
        {
            BattleEntity entity = target as BattleEntity;
            entity.StartTurn().Forget();
        }
    }
}