using System;
using UnityEngine;
using UnityUtils;

public class DropShadow : MonoBehaviour
{
    [SerializeField] private GameObject shadow;
    private RaycastHit hit;
    [SerializeField] private float offset;

    private void Update()
    {
        Ray downRay = new Ray(transform.position.With(y: transform.position.y + offset), Vector3.down);
        if (Physics.Raycast(downRay, out hit))
            shadow.transform.position = hit.point; 
    }
}
