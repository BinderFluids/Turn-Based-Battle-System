
using UnityEngine;

public abstract class SocketBattleStep : BuildableBattleActionStep
{
    [SerializeField] protected SocketReference m_socket;

    protected override void OnInitialize()
    {
        if (ParentAction == null || ParentAction.SocketData == null) return;

        if (m_socket == null)
            m_socket = new SocketReference(ParentAction.SocketData); 
        else
            m_socket.Update(ParentAction.SocketData);
    }
}