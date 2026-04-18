using UnityEngine;

public class DraggableCursor : MonoBehaviour
{
    [SerializeField] private Camera targetCamera;
    [SerializeField] private LayerMask draggableLayers = ~0;
    [SerializeField] private float maxRayDistance = 1000f;

    private Draggable3d selectedDraggable;
    private Plane dragPlane;
    private Vector3 dragOffset;
    private bool isDragging;

    private void Awake()
    {
        if (targetCamera == null)
        {
            targetCamera = Camera.main;
        }
    }

    private void Update()
    {
        if (targetCamera == null)
        {
            return;
        }

        if (Input.GetMouseButtonDown(0))
        {
            TryBeginDrag();
        }

        if (isDragging && Input.GetMouseButton(0))
        {
            UpdateDrag();
        }

        if (isDragging && Input.GetMouseButtonUp(0))
        {
            EndDrag();
        }
    }

    private void TryBeginDrag()
    {
        Ray ray = targetCamera.ScreenPointToRay(Input.mousePosition);

        if (!Physics.Raycast(ray, out RaycastHit hit, maxRayDistance, draggableLayers, QueryTriggerInteraction.Collide))
        {
            return;
        }

        Draggable3d draggable = hit.collider.GetComponentInParent<Draggable3d>();
        if (draggable == null)
        {
            return;
        }

        selectedDraggable = draggable;
        isDragging = true;

        // Plane parallel to the camera/view plane, passing through the object.
        dragPlane = new Plane(-targetCamera.transform.forward, selectedDraggable.transform.position);

        if (dragPlane.Raycast(ray, out float enter))
        {
            Vector3 hitPointOnPlane = ray.GetPoint(enter);
            dragOffset = selectedDraggable.transform.position - hitPointOnPlane;
        }
        else
        {
            dragOffset = Vector3.zero;
        }
    }

    private void UpdateDrag()
    {
        if (selectedDraggable == null)
        {
            return;
        }

        Ray ray = targetCamera.ScreenPointToRay(Input.mousePosition);

        if (dragPlane.Raycast(ray, out float enter))
        {
            Vector3 hitPointOnPlane = ray.GetPoint(enter);
            selectedDraggable.MoveTo(hitPointOnPlane + dragOffset);
        }
    }

    private void EndDrag()
    {
        isDragging = false;
        selectedDraggable = null;
        dragOffset = Vector3.zero;
    }
}