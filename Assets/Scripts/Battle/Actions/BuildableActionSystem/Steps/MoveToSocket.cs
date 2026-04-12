using System;
using System.Collections;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class MoveToSocket : BuildableBattleActionStep
{
    private enum MoveType { Speed, Duration }

    [Tooltip("Choose whether to move at a constant speed or over a duration of time")]
    [SerializeField] private MoveType moveType;
    [SerializeField, Min(0)] private float movement;  
    [SerializeField] private SocketReference socketReference;
    
    public override async UniTask Execute(BattleEntity actor, BattleEntity target)
    {
        Vector3 goal = socketReference.GetSocketPosition(); 

        if (moveType == MoveType.Duration) await UniTask.WaitForSeconds(movement); 
        //if movetype == speed then yield an actor's movement function yield return controller.MoveTo(goal, m_speed);
    }
}