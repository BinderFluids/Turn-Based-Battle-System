using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;

[CustomPropertyDrawer(typeof(AnimationTriggerReference))]
public class AnimationTriggerReferenceEditor : PropertyDrawer
{
    private const float FieldSpacing = 4f;
    private const float FullObjectFieldMinWidth = 180f;

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        SerializedProperty animatorProperty = property.FindPropertyRelative("animator");
        SerializedProperty selectedTriggerNameProperty = property.FindPropertyRelative("selectedTriggerName");

        EditorGUI.BeginProperty(position, label, property);

        Rect contentRect = EditorGUI.PrefixLabel(position, label);

        AnimatorController animatorController = animatorProperty.objectReferenceValue as AnimatorController;
        bool hasValidAnimator = animatorController != null;

        if (!hasValidAnimator)
        {
            // Full width object field until a valid controller is assigned.
            animatorProperty.objectReferenceValue = EditorGUI.ObjectField(
                contentRect,
                animatorProperty.objectReferenceValue,
                typeof(AnimatorController),
                false
            );

            EditorGUI.EndProperty();
            return;
        }

        List<string> triggerNames = FetchAnimationTriggerParameters(animatorController);

        if (triggerNames.Count == 0)
        {
            EditorGUI.HelpBox(contentRect, "Animator has no trigger parameters.", MessageType.Error);
            EditorGUI.EndProperty();
            return;
        }

        float animatorWidth = Mathf.Max(FullObjectFieldMinWidth, contentRect.width * 0.30f);
        float popupWidth = Mathf.Max(80f, contentRect.width - animatorWidth - FieldSpacing);

        Rect animatorRect = new Rect(contentRect.x, contentRect.y, animatorWidth, EditorGUIUtility.singleLineHeight);
        Rect popupRect = new Rect(
            contentRect.x + animatorWidth + FieldSpacing,
            contentRect.y,
            popupWidth,
            EditorGUIUtility.singleLineHeight
        );

        // Left: animator controller field
        animatorProperty.objectReferenceValue = EditorGUI.ObjectField(
            animatorRect,
            animatorProperty.objectReferenceValue,
            typeof(AnimatorController),
            false
        );

        // Right: trigger dropdown
        string selectedTriggerName = selectedTriggerNameProperty.stringValue;
        int chosenIndex = Mathf.Max(0, triggerNames.IndexOf(selectedTriggerName));

        int newIndex = EditorGUI.Popup(popupRect, chosenIndex, triggerNames.ToArray());
        if (newIndex >= 0 && newIndex < triggerNames.Count)
        {
            selectedTriggerNameProperty.stringValue = triggerNames[newIndex];
        }

        EditorGUI.EndProperty();
    }

    private List<string> FetchAnimationTriggerParameters(AnimatorController animatorController)
    {
        if (animatorController == null)
            throw new ArgumentException(nameof(animatorController));

        AnimatorControllerParameter[] animationParameters = animatorController.parameters;
        List<string> triggerNames = new();

        foreach (AnimatorControllerParameter parameter in animationParameters)
        {
            if (parameter.type != AnimatorControllerParameterType.Trigger)
                continue;

            triggerNames.Add(parameter.name);
        }

        return triggerNames;
    }
}