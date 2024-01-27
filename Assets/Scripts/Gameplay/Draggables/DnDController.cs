using UnityEngine;

public class DnDController : MonoBehaviour
{
    private const int grabLayer = 1 << 6;
    private const int dropLayer = 1 << 7;
    private const float rayDepth = 10.0f;
    private const float rayDistance = 100.0f;

    [SerializeField] private Camera camera;

    private Draggable dragging;

    private Vector3 grabPosition;

    private Vector3 CameraPosition => camera.transform.position;
    private Vector3 MouseWorldPosition => camera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, rayDepth));

    private void Update()
    {
        Drag();
        CheckInput();
    }

    private void CheckInput()
    {
        if (Input.GetButtonDown("Fire1")) Grab();
        if (Input.GetButtonUp("Fire1")) Drop();
    }

    private bool CastRay(out RaycastHit hitInfo, bool drop)
    {
        Vector3 cameraToMouseDirection = (MouseWorldPosition - CameraPosition).normalized;
        return Physics.Raycast(CameraPosition, cameraToMouseDirection, out hitInfo, rayDistance, drop ? dropLayer : grabLayer);
    }

    private void Grab()
    {
        if (CastRay(out RaycastHit hitInfo, false))
        {
            Draggable draggable = hitInfo.collider.GetComponent<Draggable>();
            if (draggable && !draggable.Locked)
            {
                dragging = draggable;
                dragging.OnGrabbed();

                grabPosition = dragging.transform.position;
            }
        }
    }

    private void Drag()
    {
        if (!dragging) return;

        dragging.transform.position = MouseWorldPosition;
    }

    private void Drop()
    {
        if (!dragging) return;

        dragging.OnDropped();

        if (CastRay(out RaycastHit hitInfo, true))
        {
            DropTarget target = hitInfo.collider.GetComponent<DropTarget>();
            if (target)
            {
                if (!target.TrySetContent(dragging)) dragging.transform.position = grabPosition;
            }
        }

        dragging = null;
    }
}