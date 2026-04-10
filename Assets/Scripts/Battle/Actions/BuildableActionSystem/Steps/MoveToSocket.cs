using System;
using System.Collections;
using Cysharp.Threading.Tasks;
using UnityEngine;



public class MoveToSocket : SocketBattleStep
{
    [SerializeField] private bool m_bSnapToGround;
    [SerializeField] private float m_speed;
    
    public override async UniTask Execute(BattleEntity actor, BattleEntity target)
    {
        Vector3 goal = ParentAction.SocketData.GetSocketPositionsDictionary()[m_socket.selectedSocketName]; 
        if (m_bSnapToGround) goal.y = actor.transform.position.y;

        await UniTask.WaitForSeconds(2); 
        //yield return controller.MoveTo(goal, m_speed);
    }

    public override string GetListDisplayName() => $"Move to Socket: {m_socket.selectedSocketName}";
}