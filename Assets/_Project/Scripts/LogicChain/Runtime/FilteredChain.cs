using UnityEngine;

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