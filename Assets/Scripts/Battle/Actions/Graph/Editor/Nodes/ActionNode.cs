using System;
using Unity.GraphToolkit.Editor;

[Serializable]
internal class ActionNode : BattleActionNode
{
    private const int MAX_TRANSITIONS = 2;
    private const string TRANSITION_PORT_PREFIX = "Transition"; 
    
    protected override void OnDefinePorts(IPortDefinitionContext context)
    {
        context.AddInputPort(EXECUTION_PORT_DEFAULT_NAME)
            .WithDisplayName(string.Empty)
            .WithConnectorUI(PortConnectorUI.Arrowhead)
            .Build();

        for (int i = 0; i < MAX_TRANSITIONS; i++)
        {
            context.AddOutputPort($"{TRANSITION_PORT_PREFIX}{i}")
                .WithDisplayName($"Transition {i + 1}")
                .WithConnectorUI(PortConnectorUI.Arrowhead)
                .Build();
        }
    }

    protected override void OnDefineOptions(IOptionDefinitionContext context)
    {
        context.AddOption("stateName", typeof(string))
            .Build();
        context.AddOption("socket", typeof(SocketReference))
            .Build(); 
    }
}