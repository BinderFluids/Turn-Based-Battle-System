using Codice.Client.BaseCommands;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(PlayAnimation))]
public class PlayAnimationEditor : Editor
{
    private SerializedProperty m_yieldAnimProp;
    private SerializedProperty m_triggerNameProp;
    private SerializedProperty m_animationTriggerNamesProp;
    private SerializedProperty m_durationProp;

    void OnEnable()
    {
        m_yieldAnimProp = serializedObject.FindProperty("m_bYieldAnimation");
        m_triggerNameProp = serializedObject.FindProperty("m_animationTriggerName");
        m_animationTriggerNamesProp = serializedObject.FindProperty("m_animationTriggerNames");
        m_durationProp = serializedObject.FindProperty("m_duration");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        
        _DrawDropdown();
        
        if (GUILayout.Button("Refresh Triggers"))
            (target as PlayAnimation).FetchAnimationTriggerParameters();

        serializedObject.ApplyModifiedProperties();
    }

    private void _DrawDropdown()
    {
        //Tanner: Create dropdown options
        string[] options = new string[m_animationTriggerNamesProp.arraySize];
        for (int i = 0; i < m_animationTriggerNamesProp.arraySize; i++)
            options[i] = m_animationTriggerNamesProp.GetArrayElementAtIndex(i).stringValue;

        //Tanner: Display warning if no animation is defined in parent attack
        PlayAnimation step = target as PlayAnimation;
        if (step.animatorController == null)
        {
            EditorGUILayout.HelpBox(
                "Parent attack has no Animation Controller defined.",
                MessageType.Warning);
        }
        //Tanner: Display warning if no triggers are set
        else if (options.Length == 0)
        {
            Debug.Log(step.animatorController.name);
            EditorGUILayout.HelpBox(
                "No Animator trigger parameters found. Ensure parentAttack.animatorController is set and has Trigger parameters.",
                MessageType.Warning);
        }
        //Tanner: Finally draw dropdown
        else
        {
            EditorGUILayout.PropertyField(m_yieldAnimProp);
            EditorGUILayout.PropertyField(m_durationProp);
            
            int currentIndex = Mathf.Max(0, System.Array.IndexOf(options, m_triggerNameProp.stringValue));
            int newIndex = EditorGUILayout.Popup("Animation Trigger", currentIndex, options);

            m_triggerNameProp.stringValue = options[newIndex];
        }
    }
}