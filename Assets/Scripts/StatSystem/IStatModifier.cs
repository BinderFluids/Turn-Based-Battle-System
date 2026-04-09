public interface IStatModifier
{
    float Add(float value);
    float Multiply(float value);
}

public class PercentageStatModifier : IStatModifier
{
    private float percentage;
    public PercentageStatModifier(float percentage)
        => this.percentage = percentage;
    
    public float Add(float value) => 0; 
    public float Multiply(float value) => value * percentage;
}