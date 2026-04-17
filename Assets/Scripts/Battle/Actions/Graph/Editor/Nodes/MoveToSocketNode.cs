using System;
using Unity.GraphToolkit.Editor;

[Serializable]
internal class MoveToSocketNode : BattleActionNode
{
    protected override void OnDefinePorts(IPortDefinitionContext context)
    {
        base.OnDefinePorts(context);
        
        context.AddInputPort("speed")
            .WithDisplayName("Speed")
            .WithDataType(typeof(float))
            .WithConnectorUI(PortConnectorUI.Arrowhead)
            .Build();
        
        context.AddInputPort("socketReference")
            .WithDisplayName("Socket Reference")
            .WithDataType(typeof(SocketReference))
            .WithConnectorUI(PortConnectorUI.Arrowhead)
            .Build();
    }
}