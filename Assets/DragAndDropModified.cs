using UnityEngine;
using HighlightPlus;

public class DragAndDropModified : MonoBehaviour
{
    private bool isDragging = false;
    private Vector3 offset;
    private Camera mainCamera;
    private int activeTouchId = -1;
    public GameObject targetObject;
    public float smoothSpeed = 10f;
    private HighlightEffect highlightEffect;
    private HighlightEffect targetHighlightEffect;

    void Start()
    {
        mainCamera = Camera.main;
        highlightEffect = GetComponent<HighlightEffect>();
        if (highlightEffect != null)
        {
            highlightEffect.enabled = true; // Self-highlight enabled at start
            highlightEffect.overlayColor = Color.white;
            highlightEffect.Refresh();
        }

        if (targetObject != null)
        {
            targetHighlightEffect = targetObject.GetComponent<HighlightEffect>();
            if (targetHighlightEffect != null)
            {
                targetHighlightEffect.enabled = false; // Ensure target is NOT glowing initially
                targetHighlightEffect.overlayColor = Color.green;
                targetHighlightEffect.Refresh();
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == targetObject)
        {
            Animator animator = GetComponent<Animator>();
            if (animator != null)
            {
                animator.enabled = true;
                animator.SetTrigger("StartAnimation");
            }
            else
            {
                Debug.LogError("Animator component not found on dragged object!");
            }

            isDragging = false;
            EnableTargetHighlight(false);
            this.enabled = false;
        }
    }


    void Update()
    {
#if UNITY_EDITOR || UNITY_STANDALONE
        HandleMouseInput();
#elif UNITY_IOS || UNITY_ANDROID
        HandleTouchInput();
#endif
    }

    private void HandleMouseInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            if (Physics.Raycast(mainCamera.ScreenPointToRay(Input.mousePosition), out hit) && hit.transform == transform)
            {
                offset = gameObject.transform.position - GetMouseWorldPos(Input.mousePosition);
                isDragging = true;
                EnableSelfHighlight(false);
                EnableTargetHighlight(true);
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            isDragging = false;
            Handheld.Vibrate();
            EnableTargetHighlight(false);
        }

        if (isDragging)
        {
            Vector3 targetPos = GetMouseWorldPos(Input.mousePosition) + offset;
            targetPos.z = transform.position.z; // Lock movement in Z-axis
            transform.position = Vector3.Lerp(transform.position, targetPos, Time.deltaTime * smoothSpeed);
        }
    }

    private void HandleTouchInput()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            switch (touch.phase)
            {
                case TouchPhase.Began:
                    RaycastHit hit;
                    if (Physics.Raycast(mainCamera.ScreenPointToRay(touch.position), out hit) && hit.transform == transform)
                    {
                        offset = gameObject.transform.position - GetMouseWorldPos(touch.position);
                        isDragging = true;
                        activeTouchId = touch.fingerId;
                        EnableSelfHighlight(false);
                        EnableTargetHighlight(true);
                    }
                    break;

                case TouchPhase.Moved:
                    if (isDragging && touch.fingerId == activeTouchId)
                    {
                        Vector3 targetPos = GetMouseWorldPos(touch.position) + offset;
                        targetPos.z = transform.position.z; // Lock movement in Z-axis
                        transform.position = Vector3.Lerp(transform.position, targetPos, Time.deltaTime * smoothSpeed);
                    }
                    break;

                case TouchPhase.Ended:
                case TouchPhase.Canceled:
                    if (touch.fingerId == activeTouchId)
                    {
                        isDragging = false;
                        activeTouchId = -1;
                        Handheld.Vibrate();
                        EnableTargetHighlight(false);
                    }
                    break;
            }
        }
    }

    private Vector3 GetMouseWorldPos(Vector3 screenPosition)
    {
        screenPosition.z = mainCamera.WorldToScreenPoint(gameObject.transform.position).z;
        return mainCamera.ScreenToWorldPoint(screenPosition);
    }

    public void EnableSelfHighlight(bool enable)
    {
        if (highlightEffect != null)
        {
            highlightEffect.enabled = enable;
            highlightEffect.overlayColor = enable ? Color.white : Color.clear;
            highlightEffect.Refresh();
        }
    }

    public void EnableTargetHighlight(bool enable)
    {
        if (targetHighlightEffect != null)
        {
            targetHighlightEffect.enabled = enable;
            targetHighlightEffect.overlayColor = enable ? Color.green : Color.clear;
            targetHighlightEffect.Refresh();
        }
    }

    private void OnDisable()
    {
        EnableSelfHighlight(false);
        EnableTargetHighlight(false);
    }
}
