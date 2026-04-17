using System;
using Unity.GraphToolkit.Editor;

namespace Battle.Actions.Graph.Editor.Nodes
{
    [Serializable]
    internal class StartNode : BattleActionNode
    {
        protected override void OnDefinePorts(IPortDefinitionContext context)
        {
            context.AddOutputPort("Out0")
                .WithDisplayName("Out (0)")
                .WithConnectorUI(PortConnectorUI.Arrowhead)
                .Build();
        }
    }
}