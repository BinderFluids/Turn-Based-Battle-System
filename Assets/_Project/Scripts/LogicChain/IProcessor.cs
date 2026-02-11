public interface IProcessor<in TIn, out TOut>
{
    TOut Process(TIn input);
}