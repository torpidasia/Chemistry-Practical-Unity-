using UnityEngine;
using HighlightPlus;

public class DragAndDrop : MonoBehaviour
{
    private bool isDragging = false;
    private Vector3 offset;
    private Camera mainCamera;
    private int activeTouchId = -1;
    public GameObject selfArrow, targetArrow;
    public float animationSpeed = 2f;
    public float animationHeight = 0.05f;
    public Vector3 animationAxis = Vector3.up; // Default to animating along the Y axis

    private Vector3 selfArrowStartPos;
    private Vector3 targetArrowStartPos;

    public bool animateArrows = true;
    public float smoothSpeed = 10f; // Smooth dragging speed
    private HighlightEffect highlightEffect;

    void Start()
    {
        mainCamera = Camera.main;
        DeactivateArrows();
        selfArrowStartPos = selfArrow.transform.localPosition;
        targetArrowStartPos = targetArrow.transform.localPosition;
        highlightEffect = GetComponent<HighlightEffect>();
        if (highlightEffect != null)
        {
            highlightEffect.enabled = false;
        }
    }

    void Update()
    {
#if UNITY_EDITOR || UNITY_STANDALONE
        HandleMouseInput();
#elif UNITY_IOS || UNITY_ANDROID
        HandleTouchInput();
#endif
        AnimateArrows();
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
                EnableHighlight(true);
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            isDragging = false;
            Handheld.Vibrate(); // Haptic feedback on drag end
            EnableHighlight(false);
        }

        if (isDragging)
        {
            Vector3 targetPos = GetMouseWorldPos(Input.mousePosition) + offset;
            transform.position = Vector3.Lerp(transform.position, targetPos, Time.deltaTime * smoothSpeed);
        }

        if (animateArrows)
        {
            selfArrow.SetActive(!isDragging);
            targetArrow.SetActive(isDragging);
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
                        EnableHighlight(true);
                    }
                    break;

                case TouchPhase.Moved:
                    if (isDragging && touch.fingerId == activeTouchId)
                    {
                        Vector3 targetPos = GetMouseWorldPos(touch.position) + offset;
                        transform.position = Vector3.Lerp(transform.position, targetPos, Time.deltaTime * smoothSpeed);
                    }
                    break;

                case TouchPhase.Ended:
                case TouchPhase.Canceled:
                    if (touch.fingerId == activeTouchId)
                    {
                        isDragging = false;
                        activeTouchId = -1;
                        Handheld.Vibrate(); // Haptic feedback on drag end
                        EnableHighlight(false);
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

    private void AnimateArrows()
    {
        float newY = Mathf.Sin(Time.time * animationSpeed) * animationHeight;
        Vector3 animationOffset = animationAxis * newY;

        if (animateArrows)
        {
            if (selfArrow.activeSelf)
            {
                selfArrow.transform.localPosition = selfArrowStartPos + animationOffset;
            }

            if (targetArrow.activeSelf)
            {
                targetArrow.transform.localPosition = targetArrowStartPos + animationOffset;
            }
        }
    }

    private void EnableHighlight(bool enable)
    {
        if (highlightEffect != null)
        {
            highlightEffect.enabled = enable;
        }
    }

    public void DeactivateArrows()
    {
        selfArrow.SetActive(false);
        targetArrow.SetActive(false);
    }

    public void SetArrowActivationMode(bool isActive)
    {
        animateArrows = isActive;
        selfArrow.SetActive(isActive);
        targetArrow.SetActive(isActive);
    }

    private void OnDisable()
    {
        DeactivateArrows();
        EnableHighlight(false);
    }

    public void SwitchTarget(GameObject changeWith)
    {
        DeactivateArrows();
        targetArrow = changeWith;
        selfArrowStartPos = selfArrow.transform.localPosition;
        targetArrowStartPos = targetArrow.transform.localPosition;
    }
}
