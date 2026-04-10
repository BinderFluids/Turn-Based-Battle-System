using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEditor.Animations;
using UnityEngine;


public class PlayAnimation : BuildableBattleActionStep
{
    [SerializeField, Tooltip("Whether or not to wait for the animation to finish before moving on to the next step")] 
    private bool m_bYieldAnimation; 
    
    [Tooltip("If the animator controller is somehow unspecified, this will be used as the duration of the animation")]
    [SerializeField] private float m_duration = 1f;
    [SerializeField] private int m_animationIndex;
    public int animationIndex { get => m_animationIndex; set => m_animationIndex = value;}
    
    [SerializeField] private string m_animationTriggerName;
    [SerializeField] private List<string> m_animationTriggerNames = new List<string>();
    public AnimatorController animatorController => ParentAction.AnimatorController;

    protected override void OnInitialize()
    {
        FetchAnimationTriggerParameters();
    }

    public override async UniTask Execute(BattleEntity actor, BattleEntity target)
    {
        if (!string.IsNullOrEmpty(m_animationTriggerName) /*&& controller.Anim != null*/)
        {
            Animator anim = new(); //TODO: PASS IN ANIMATOR CONTROLLER FROM ACTOR
            anim.SetTrigger(m_animationTriggerName);
            if (m_bYieldAnimation) await UniTask.WaitForSeconds(anim.GetCurrentAnimatorStateInfo(0).length);
        }
        else
        {
            Debug.LogWarning("Animation trigger is empty on attack step, or enemy controller has no animator.");
            await UniTask.WaitForSeconds(m_duration);
        }
    }

    public override string GetListDisplayName() => $"Play Animation {m_animationTriggerName}";
    
    public void FetchAnimationTriggerParameters()
    {
        if (animatorController == null)
        {
            Debug.LogError($"Enemy Attack Scriptable Object {ParentAction.name} has no animator controller assigned.");
            return;
        }
        
        AnimatorControllerParameter[] animationParameters = animatorController.parameters;
        m_animationTriggerNames.Clear();

        foreach (AnimatorControllerParameter parameter in animationParameters)
        {
            if (parameter.type != AnimatorControllerParameterType.Trigger) continue;
            m_animationTriggerNames.Add(parameter.name);
        }
    }
    
}