public delegate TChain ChainFactory<out TIn, in TOut, out TChain>(IProcessor<TIn, TOut> processor)
    where TChain : FluentChain<TIn, TOut, TChain>;