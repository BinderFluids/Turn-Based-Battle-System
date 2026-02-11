using UnityEngine;

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