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