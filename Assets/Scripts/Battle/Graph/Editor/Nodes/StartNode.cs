using System;
using Unity.GraphToolkit.Editor;

namespace Battle.Graph.Editor.Battle.Nodes
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