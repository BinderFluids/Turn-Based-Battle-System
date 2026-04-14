using System;
using UnityEditor.Animations;
using UnityEngine;

[Serializable]
public class AnimationTriggerReference
{
    [SerializeField, HideInInspector] private AnimatorController animator;
    public AnimatorController Animator => animator;
    
    [SerializeField, HideInInspector] private string selectedTriggerName;
    public string SelectedTriggerName => selectedTriggerName;
}