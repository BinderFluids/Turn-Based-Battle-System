public class DistanceScorer : IProcessor<float, float>
{
    public float Process(float distance) => 1f / (1f + distance);
}