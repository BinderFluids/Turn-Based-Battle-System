using System.Collections.Generic;
using Battle.Actions.Graph.Runtime.Nodes;
using UnityEngine;

namespace Battle.Actions.Graph.Runtime
{
    public class BattleActionRuntimeGraph : ScriptableObject
    {
        [SerializeReference] public List<RuntimeNode> Nodes = new();
    }
}