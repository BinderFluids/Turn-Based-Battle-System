using System;
using UnityEngine;

public static class Chain
{
    public static DistanceChain FromPlayer(Transform player) => new DistanceChain(new DistanceFromPlayer(player));
    public static DistanceChain Start(DistanceFromPlayer processor) => new DistanceChain(processor);
    //public static DistanceChain Start<TProcessor>(TProcessor processor) where TProcessor : IProcessor<Vector3, float> => new DistanceChain(processor);
}

public class HighlightIfClose : IProcessor<bool, bool>
{
    private readonly Transform transform;

    public HighlightIfClose(Transform target) => transform = target;

    public bool Process(bool isCloseEnough)
    {
        var renderer = transform.GetComponent<Renderer>();
        renderer.material.color = isCloseEnough ? Color.yellow : Color.white;
        return isCloseEnough;
    }
}

public class FilteredChain : FluentChain<Vector3, bool, FilteredChain>
{
    public FilteredChain(IProcessor<Vector3, bool> processor) : base(processor) { }

    static FilteredChain Create(IProcessor<Vector3, bool> processor)
    {
        return new FilteredChain(processor);
    }

    public FilteredChain LogTo(string system)
    {
        Debug.Log($"#{system}# Filtered Chain!");
        return this; 
    }
    
    public FilteredChain Then<TProcessor>(TProcessor next) where TProcessor : class, IProcessor<bool, bool>
        => Then<bool, FilteredChain, TProcessor>(next, Create);
}

public class ScoredChain : FluentChain<Vector3, float, ScoredChain>
{
    public ScoredChain(IProcessor<Vector3, float> processor) : base(processor) { }

    static FilteredChain CreateFilteredChain(IProcessor<Vector3, bool> processor)
    {
        return new FilteredChain(processor);
    }

    public ScoredChain WithMaxDistance(float maxDist)
    {
        processor = new Combined<Vector3, float, float>(processor, new ClampByMaxDistance(maxDist));
        return new ScoredChain(processor);
    }
    
    public FilteredChain Then<TProcessor>(TProcessor scorer) where TProcessor : class, IProcessor<float, bool>
        => Then<bool, FilteredChain, TProcessor>(scorer, CreateFilteredChain);
}

class ClampByMaxDistance : IProcessor<float, float>
{
    private readonly float maxDistanceScoreThreshold;

    public ClampByMaxDistance(float maxDistance)
    {
        maxDistanceScoreThreshold = 1f / (1f + maxDistance);
    }

    public float Process(float score) => score < maxDistanceScoreThreshold ? 0f : score; 
}

public class DistanceChain : FluentChain<Vector3, float, DistanceChain>
{
    public DistanceChain(IProcessor<Vector3, float> processor) : base(processor) { }

    static ScoredChain CreateScoredChain(IProcessor<Vector3, float> processor)
    {
        return new ScoredChain(processor); 
    }
    
    public ScoredChain Then<TProcessor>(TProcessor scorer) where TProcessor : class, IProcessor<float, float>
        => Then<float, ScoredChain, TProcessor>(scorer, CreateScoredChain);
}

public abstract class FluentChain<TIn, TOut, TDerived> where TDerived : FluentChain<TIn, TOut, TDerived>
{
    public IProcessor<TIn, TOut> processor;

    protected FluentChain(IProcessor<TIn, TOut> processor)
    {
        this.processor = processor ?? throw new ArgumentNullException(nameof(processor));
    }

    protected TNextSelf Then<TNext, TNextSelf, TProcessor>(
        TProcessor nextProcessor,
        ChainFactory<TIn, TNext, TNextSelf> factory)
        where TNextSelf : FluentChain<TIn, TNext, TNextSelf>
        where TProcessor : IProcessor<TOut, TNext>
    {
        if (nextProcessor == null) throw new ArgumentNullException(nameof(nextProcessor));
        if (factory == null) throw new ArgumentNullException(nameof(factory));

        return factory(new Combined<TIn, TOut, TNext>(processor, nextProcessor));
    }

    public TOut Run(TIn input)
    {
        if (processor == null) throw new InvalidOperationException("Processor not set. Use Chain.Start() to begin a chain.");
        return processor.Process(input);
    }

    public ProcessorDelegate<TIn, TOut> Compile()
    {
        if (processor == null) throw new InvalidOperationException("Processor not set. Use Chain.Start() to begin a chain.");
        return input => processor.Process(input);
    }
}

public delegate TChain ChainFactory<out TIn, in TOut, out TChain>(IProcessor<TIn, TOut> processor)
    where TChain : FluentChain<TIn, TOut, TChain>;