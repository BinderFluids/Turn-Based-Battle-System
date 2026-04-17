using System;
using Unity.GraphToolkit.Editor;

namespace Battle.Actions.Graph.Editor.Nodes
{
    [Serializable]
    internal abstract class BattleActionNode : Node
    {
        public const string EXECUTION_PORT_DEFAULT_NAME = "ExecutionPort";

        protected override void OnDefinePorts(IPortDefinitionContext context)
        {
            context.AddInputPort(EXECUTION_PORT_DEFAULT_NAME)
                .WithDisplayName(string.Empty)
                .WithConnectorUI(PortConnectorUI.Arrowhead)
                .Build();
        
            context.AddOutputPort("Out0")
                .WithDisplayName("Out (0)")
                .WithConnectorUI(PortConnectorUI.Arrowhead)
                .Build();
        }
    }
}