using System;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityUtils;

public class FreelookCamera : MonoBehaviour
{
    [SerializeField] private Transform _transform; 
    [SerializeField] private Camera camera;
    [SerializeField] private CinemachineInputAxisController cinemachineInputAxisController;
    [SerializeField] private SocketEditorInputReader input;
    [SerializeField] private float rotateSpeed; 
    [SerializeField] private float dragSpeed;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        input.EnableInput(InputActionType.Freelook);
        
        _transform ??= transform;
        camera ??= Camera.main;
    }

    // Update is called once per frame
    private Vector3 dragOrigin;
    private bool isPanning;

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(_transform.position, _transform.position + _transform.forward * 10); 
    }

    void Update()
    {
        cinemachineInputAxisController.Controllers[0].Enabled = input.ActivateCameraRotation.Value;
        cinemachineInputAxisController.Controllers[1].Enabled = input.ActivateCameraRotation.Value;

        _transform.Translate(_transform.forward * input.ForwardMovement.Value, Space.World); 

        if (!input.ActivateCameraPan.Value)
        {
            isPanning = false;
            return;
        }

        if (!isPanning)
        {
            dragOrigin = Input.mousePosition;
            isPanning = true;
            return;
        }

        Vector3 delta = Input.mousePosition - dragOrigin;
        Vector3 move = new Vector3(delta.x * dragSpeed, delta.y * dragSpeed, 0f);

        transform.Translate(-move, Space.Self);

        dragOrigin = Input.mousePosition;
    }

}
