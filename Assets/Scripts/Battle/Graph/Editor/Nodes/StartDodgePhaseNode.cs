using Battle.Interfaces;
using SerializedInterface;
using UnityEngine;

namespace Battle.Graph.Editor.Battle.Nodes
{
    internal class StartDodgePhaseNode : BattleActionNode
    {
        [SerializeField] private InterfaceReference<IDodgeBehaviour> dodgeBehaviourRef; 
    }
}