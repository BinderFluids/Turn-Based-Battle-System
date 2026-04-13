using System;
using System.Collections.Generic;
using EventBus;

/// <summary>
/// Raised when a responsive input window opens (no fixed duration; closed via <see cref="CloseWindow"/> or service).
/// </summary>
public class ResponsiveWindowOpened : IEvent
{
    public string WindowId { get; }
    public IReadOnlyList<Battle.Input.InputType> ExpectedInputs { get; }
    public string Label { get; }

    public ResponsiveWindowOpened(
        string windowId,
        IReadOnlyList<Battle.Input.InputType> expectedInputs,
        string label = null)
    {
        WindowId = windowId;
        ExpectedInputs = expectedInputs ?? Array.Empty<Battle.Input.InputType>();
        Label = label;
    }
}
