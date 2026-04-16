using Unity.GraphToolkit.Editor;

internal class MoveToSocketNode : BattleActionNode
{
    protected override void OnDefinePorts(IPortDefinitionContext context)
    {
        base.OnDefinePorts(context);
        
        context.AddInputPort("socketReference")
            .WithDisplayName("Socket Reference")
            .WithDataType(typeof(SocketReference))
            .WithConnectorUI(PortConnectorUI.Arrowhead)
            .Build();
    }
}