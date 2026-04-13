using System;
using System.Collections.Generic;
using EventBus;

/// <summary>
/// Raised when a timed action command window starts. UI should show prompts and timing feedback.
/// </summary>
public class ActionCommandWindowOpened : IEvent
{
    public string WindowId { get; }
    public float DurationSeconds { get; }
    public IReadOnlyList<Battle.Input.InputType> ExpectedInputs { get; }

    /// <summary>Relative times (seconds from window start) when input counts as valid.</summary>
    public float ValidInputStartTime { get; }
    public float ValidInputEndTime { get; }
}
