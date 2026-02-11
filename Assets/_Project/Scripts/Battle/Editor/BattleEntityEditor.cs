
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(BattleEntity))]
public class BattleEntityEditor : Editor
{
    public override void OnInspectorGUI()
    {
        BattleEntity entity = target as BattleEntity;
        
        base.OnInspectorGUI();
        if (GUILayout.Button("Start Turn"))
            entity.StartTurn().Forget();
    }
}