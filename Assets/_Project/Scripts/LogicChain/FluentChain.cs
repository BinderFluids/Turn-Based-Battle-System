using System;
using UnityEngine;

public static class Chain
{
    public static DistanceChain FromPlayer(Transform player) => new DistanceChain(new DistanceFromPlayer(player));
    public static DistanceChain Start(DistanceFromPlayer processor) => new DistanceChain(processor);
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