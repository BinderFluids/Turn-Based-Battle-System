using Unity.GraphToolkit.Editor;

internal class WaitNode : BattleActionNode
{
    protected override void OnDefinePorts(IPortDefinitionContext context)
    {
        base.OnDefinePorts(context);
        
        context.AddInputPort("waitTime")
            .WithDisplayName("Wait Time")
            .WithDataType(typeof(float))
            .WithConnectorUI(PortConnectorUI.Arrowhead)
            .Build();
    }
}