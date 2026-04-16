using System.Collections.Generic;
using UnityEngine;

public class BattleActionRuntimeGraph : ScriptableObject
{
    [SerializeReference] public List<RuntimeNode> Nodes = new(); 
}