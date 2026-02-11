using System;
using System.Collections.Generic;

public class ThresholdFilter : IProcessor<float, bool>
{
    private readonly Func<float> getThreshold;
    
    public ThresholdFilter(Func<float> getThreshold) => this.getThreshold = getThreshold;
    
    public bool Process(float input) => input > getThreshold();
}