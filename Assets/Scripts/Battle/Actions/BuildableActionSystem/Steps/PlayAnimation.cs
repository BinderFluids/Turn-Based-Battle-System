using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEditor.Animations;
using UnityEngine;

[Serializable]
public class PlayAnimation : BuildableBattleActionStep
{
    [SerializeField, Tooltip("Whether or not to wait for the animation to finish before moving on to the next step")]
    private bool yieldAnimation;  
    
    [Tooltip("If the animator controller is somehow unspecified, this will be used as the duration of the animation")]
    [SerializeField] private float m_duration = 1f;

    [SerializeField] protected AnimationTriggerReference animationTriggerReference;
    
    public override async UniTask Execute(BattleEntity actor, BattleEntity target)
    {
        if (!string.IsNullOrEmpty(animationTriggerReference.SelectedTriggerName) /*&& controller.Anim != null*/)
        {
            Animator anim = new(); //TODO: PASS IN ANIMATOR CONTROLLER FROM ACTOR
            anim.SetTrigger(animationTriggerReference.SelectedTriggerName);
            if (yieldAnimation) await UniTask.WaitForSeconds(anim.GetCurrentAnimatorStateInfo(0).length);
        }
        else
        {
            Debug.LogWarning("Animation trigger is empty on attack step, or enemy controller has no animator.");
            await UniTask.WaitForSeconds(m_duration);
        }
    }
}