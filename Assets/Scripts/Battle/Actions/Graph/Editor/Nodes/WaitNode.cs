using System;
using Unity.GraphToolkit.Editor;

namespace Battle.Actions.Graph.Editor.Nodes
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