using UnityEngine;

public class HighlightIfClose : IProcessor<bool, bool>
{
    private readonly Transform transform;

    public HighlightIfClose(Transform target) => transform = target;

    public bool Process(bool isCloseEnough)
    {
        var renderer = transform.GetComponent<Renderer>();
        renderer.material.color = isCloseEnough ? Color.yellow : Color.white;
        return isCloseEnough;
    }
}