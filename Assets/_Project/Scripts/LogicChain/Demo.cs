using System;
using UnityEngine;

public class Demo : MonoBehaviour
{
    private ProcessorDelegate<Vector3, bool> shouldAttack;
    private ProcessorDelegate<Vector3, bool> highlightClose;

    private float distanceThreshold = .5f;
    public Transform target;

    private void Start()
    {
        shouldAttack = Chain.Start(new DistanceFromPlayer(transform))
            .Then(new DistanceScorer())
            .WithMaxDistance(15f)
            .Then(new ThresholdFilter(() => distanceThreshold))
            .LogTo("Score")
            .LogTo("Proximity")
            .Compile(); 
    }
}