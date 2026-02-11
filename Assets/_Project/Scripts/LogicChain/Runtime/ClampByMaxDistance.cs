class ClampByMaxDistance : IProcessor<float, float>
{
    private readonly float maxDistanceScoreThreshold;

    public ClampByMaxDistance(float maxDistance)
    {
        maxDistanceScoreThreshold = 1f / (1f + maxDistance);
    }

    public float Process(float score) => score < maxDistanceScoreThreshold ? 0f : score; 
}