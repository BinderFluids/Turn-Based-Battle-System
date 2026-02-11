
using System;
using UnityEngine;

public interface IProcessor<in TIn, out TOut>
{
    TOut Process(TIn input);
}
public delegate TOut ProcessorDelegate<TIn, TOut>(TIn input);

class Chain<TIn, TOut>
{
    private readonly IProcessor<TIn, TOut> processor;
    
    Chain(IProcessor<TIn, TOut> processor) => this.processor = processor;

    public static Chain<TIn, TOut> Start(IProcessor<TIn, TOut> processor)
    {
        return new Chain<TIn, TOut>(processor);
    }

    public Chain<TIn, TNext> Then<TNext>(IProcessor<TOut, TNext> next)
    {
        var combined = new Combined<TIn, TOut, TNext>(processor, next);
        return new Chain<TIn, TNext>(combined);
    }
    
    public TOut Run(TIn input) => processor.Process(input);
    public ProcessorDelegate<TIn, TOut> Compile() => input => processor.Process(input);
}

class Combined<A, B, C> : IProcessor<A, C>
{
    private readonly IProcessor<A, B> first;
    private readonly IProcessor<B, C> second;
    
    public Combined(IProcessor<A, B> first, IProcessor<B, C> second)
    {
        this.first = first;
        this.second = second;
    }

    public C Process(A input) => second.Process(first.Process(input));
}

public class ThresholdFilter : IProcessor<float, bool>
{
    private readonly Func<float> getThreshold;
    
    public ThresholdFilter(Func<float> getThreshold) => this.getThreshold = getThreshold;
    
    public bool Process(float input) => input > getThreshold();
}

public class DistanceScorer : IProcessor<float, float>
{
    public float Process(float distance) => 1f / (1f + distance);
}

public class DistanceFromPlayer : IProcessor<Vector3, float>
{
    private readonly Transform player;
    
    public DistanceFromPlayer(Transform player) => this.player = player;
    
    public float Process(Vector3 input) => Vector3.Distance(input, player.position);
}

