using System.Collections.Generic;
using Battle.Graph.Runtime.Nodes;
using UnityEngine;

namespace Battle.Graph.Runtime
{
    public class BattleActionRuntimeGraph : ScriptableObject
    {
        [SerializeReference] public List<RuntimeNode> Nodes = new();
    }
}