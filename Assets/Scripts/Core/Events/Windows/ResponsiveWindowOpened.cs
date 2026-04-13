using System;
using System.Collections.Generic;
using Battle.BattleWindow.Enums;
using EventBus;

/// <summary>
/// Raised when a responsive input window opens (no fixed duration; closed via <see cref="CloseWindow"/> or service).
/// </summary>
public class ResponsiveWindowOpened : IEvent
{
    public string WindowId { get; }
    public IReadOnlyList<PlayerId> ExpectedInputs { get; }
    public string Label { get; }

    public ResponsiveWindowOpened(  
        string windowId,
        IReadOnlyList<PlayerId> expectedInputs,
        string label = null)
    {
        WindowId = windowId;
        ExpectedInputs = expectedInputs ?? Array.Empty<PlayerId>();
        Label = label;
    }
}
