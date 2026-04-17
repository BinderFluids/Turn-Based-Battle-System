using System;
using UnityEngine;

public class ConstantScreenSize : MonoBehaviour
{
    [SerializeField] private Transform camera;
    [SerializeField] private Transform _transform; 
    [SerializeField, Range(0.03f, 0.1f)] private float targetSize;

    private void Update()
    {
        var size = (camera.position - transform.position).magnitude;
        _transform.localScale = Vector3.one * targetSize * size;
    }
}
