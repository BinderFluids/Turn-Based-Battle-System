using System;
using Unity.Cinemachine;
using UnityEngine;

public class CameraReset : MonoBehaviour
{
    [SerializeField] private Transform _transform; 
    [SerializeField] private Transform anchor; 
    [SerializeField] private CinemachinePanTilt cinemachinePanTilt;
    [SerializeField] private Vector2 panTilt; 

    private void Start()
    {
        if (_transform == null)
            _transform = transform;
    }

    public void Reset()
    {
        if (anchor == null)
        {
            Debug.LogError($"{name} is missing an anchor.");
            return;
        }

        if (cinemachinePanTilt == null)
        {
            Debug.LogError($"{name} is missing a CinemachinePanTilt.");
            return;
        }
        
        cinemachinePanTilt.PanAxis.Value = panTilt.x;
        cinemachinePanTilt.TiltAxis.Value = panTilt.y; 
        _transform.position = anchor.position;
    }
}
