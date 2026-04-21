
using System;
using UnityEngine;

public class Draggable3d : MonoBehaviour
{
    public void MoveTo(Vector3 worldPosition)
    {
        transform.position = worldPosition;
    }
}