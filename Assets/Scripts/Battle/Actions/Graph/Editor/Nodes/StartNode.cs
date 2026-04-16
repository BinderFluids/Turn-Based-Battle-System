using System;
using Unity.GraphToolkit.Editor;

[Serializable]
internal class StartNode : BattleActionNode
{
    protected override void OnDefinePorts(IPortDefinitionContext context)
    {
        context.AddOutputPort(EXECUTION_PORT_DEFAULT_NAME)
            .WithDisplayName(string.Empty)
            .WithConnectorUI(PortConnectorUI.Arrowhead)
            .Build(); 
    }
}