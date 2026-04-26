using System;
using Unity.GraphToolkit.Editor;

namespace Battle.Graph.Editor.Battle.Nodes
{
    [Serializable]
    internal class WaitNode : BattleActionNode
    {
        protected override void OnDefinePorts(IPortDefinitionContext context)
        {
            base.OnDefinePorts(context);
        
            context.AddInputPort("duration")
                .WithDisplayName("Duration")
                .WithDataType(typeof(float))
                .WithConnectorUI(PortConnectorUI.Arrowhead)
                .Build();
        }
    }
}