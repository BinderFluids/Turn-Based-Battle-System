using System;
using Unity.GraphToolkit.Editor;

[Serializable]
internal abstract class BattleActionNode : Node
{
    public const string EXECUTION_PORT_DEFAULT_NAME = "ExecutionPort";
}