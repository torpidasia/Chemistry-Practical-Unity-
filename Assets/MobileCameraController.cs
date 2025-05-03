using UnityEngine;
using UnityEngine.EventSystems;

public class MobileCameraController : MonoBehaviour
{
    public Transform startPoint;
    public Transform endPoint;
    public float moveSpeed = 2f;

    public float minZoom = 5f;
    public float maxZoom = 15f;
    public float zoomSpeed = 0.1f;

    public RectTransform rotationGizmoUI;
    public Transform rotationGizmo3D;
    public float rotationSpeed = 2f; // Reduced for better control
    public float gizmoRotationSpeed = 1.5f; // Separate speed for gizmo rotation
    public float rotationDamping = 5f; // Lowered for better gradual rotation
    public Vector2 rotationLimitX = new Vector2(-30, 30); // X rotation limit (Up/Down)
    public Vector2 rotationLimitY = new Vector2(-60, 60); // Y rotation limit (Left/Right)

    public float panSpeed = 0.1f;
    public Vector2 panLimitX = new Vector2(-10, 10);
    public Vector2 panLimitY = new Vector2(-5, 5);

    private Camera cam;
    private bool isMoving = true;
    private Vector2 lastGizmoPosition;
    private bool isRotating = false;
    private Vector2 lastPanPosition;
    private bool isPanning = false;
    private float targetXRotation = 0f;
    private float targetYRotation = 0f;

    void Start()
    {
        cam = Camera.main;
        transform.position = startPoint.position;
    }

    void Update()
    {
        HandleCameraMovement();
        HandleZoom();
        HandleRotation();
        HandlePan();
    }

    void HandleCameraMovement()
    {
        if (isMoving)
        {
            Vector3 targetPosition = new Vector3(endPoint.position.x, startPoint.position.y, endPoint.position.z);
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

            if (Vector3.Distance(transform.position, targetPosition) < 0.01f)
            {
                isMoving = false;
            }
        }
    }

    void HandleZoom()
    {
        if (Input.touchCount == 2)
        {
            Touch touchZero = Input.GetTouch(0);
            Touch touchOne = Input.GetTouch(1);

            Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
            Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

            float prevTouchDeltaMag = (touchZeroPrevPos - touchOnePrevPos).magnitude;
            float touchDeltaMag = (touchZero.position - touchOne.position).magnitude;

            float deltaMagnitudeDiff = touchDeltaMag - prevTouchDeltaMag; // <--- Flip here

            if (cam.orthographic)
            {
                cam.orthographicSize = Mathf.Clamp(cam.orthographicSize - deltaMagnitudeDiff * zoomSpeed, minZoom, maxZoom);
            }
            else
            {
                cam.fieldOfView = Mathf.Clamp(cam.fieldOfView - deltaMagnitudeDiff * zoomSpeed * 5f, minZoom, maxZoom);
            }
        }
    }



    void HandleRotation()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (RectTransformUtility.RectangleContainsScreenPoint(rotationGizmoUI, touch.position))
            {
                if (touch.phase == TouchPhase.Began)
                {
                    lastGizmoPosition = touch.position;
                    isRotating = true;
                }
                else if (touch.phase == TouchPhase.Moved && isRotating)
                {
                    Vector2 delta = touch.position - lastGizmoPosition;

                    // Apply smooth and restricted rotation
                    targetYRotation = Mathf.Clamp(targetYRotation - delta.x * rotationSpeed * Time.deltaTime, rotationLimitY.x, rotationLimitY.y);
                    targetXRotation = Mathf.Clamp(targetXRotation + delta.y * rotationSpeed * Time.deltaTime, rotationLimitX.x, rotationLimitX.y);

                    lastGizmoPosition = touch.position;
                }
                else if (touch.phase == TouchPhase.Ended)
                {
                    isRotating = false;
                }
            }
        }

        // Smoothly interpolate the camera rotation with limits
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(targetXRotation, targetYRotation, 0), Time.deltaTime * rotationDamping);

        // Slow down and limit the gizmo rotation separately
        if (rotationGizmo3D != null)
        {
            rotationGizmo3D.rotation = Quaternion.Lerp(rotationGizmo3D.rotation, Quaternion.Euler(targetXRotation, targetYRotation, 0), Time.deltaTime * gizmoRotationSpeed);
        }
    }

    void HandlePan()
    {
        if (Input.touchCount == 1)
        {
            Touch touch = Input.GetTouch(0);
            Ray ray = cam.ScreenPointToRay(touch.position);

            if (touch.phase == TouchPhase.Began && !IsTouchingInteractable(ray))
            {
                lastPanPosition = touch.position;
                isPanning = true;
            }
            else if (touch.phase == TouchPhase.Moved && isPanning)
            {
                Vector2 delta = (Vector2)touch.position - lastPanPosition;

                Vector3 newPosition = transform.position - new Vector3(delta.x * panSpeed * Time.deltaTime, delta.y * panSpeed * Time.deltaTime, 0);
                newPosition.x = Mathf.Clamp(newPosition.x, panLimitX.x, panLimitX.y);
                newPosition.y = Mathf.Clamp(newPosition.y, panLimitY.x, panLimitY.y);

                transform.position = newPosition;
                lastPanPosition = touch.position;
            }
            else if (touch.phase == TouchPhase.Ended)
            {
                isPanning = false;
            }
        }
    }

    bool IsTouchingInteractable(Ray ray)
    {
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            if (hit.collider != null && hit.collider.GetComponent<Interactable>() != null)
            {
                return true;
            }
        }
        return false;
    }
}
