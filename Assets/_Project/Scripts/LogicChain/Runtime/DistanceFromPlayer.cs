using UnityEngine;

public class DistanceFromPlayer : IProcessor<Vector3, float>
{
    private readonly Transform player;
    
    public DistanceFromPlayer(Transform player) => this.player = player;
    
    public float Process(Vector3 input) => Vector3.Distance(input, player.position);
}