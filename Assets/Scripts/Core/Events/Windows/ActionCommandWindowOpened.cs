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
    public float ThresholdSeconds { get; }

    public ActionCommandWindowOpened(string windowId, float durationSeconds, float thresholdSeconds)
    {
        WindowId = windowId;
        DurationSeconds = durationSeconds;
        ThresholdSeconds = thresholdSeconds;
    }
}