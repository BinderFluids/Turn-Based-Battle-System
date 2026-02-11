using UnityEngine;

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