using UnityEngine;

public class Stick : MonoBehaviour
{
    public void OnAnimationStart()
    {
        var dragComponent = GetComponent<DragAndDropModified>();
        if (dragComponent != null)
        {
            dragComponent.EnableSelfHighlight(false);
            dragComponent.EnableTargetHighlight(false);
            dragComponent.enabled = false; // 🔹 Temporarily disable dragging during animation
        }
    }

    public void OnAnimationComplete()
    {
        GetComponent<Animator>().enabled = false;

        var dragComponent = GetComponent<DragAndDropModified>();
        if (dragComponent != null)
        {
            dragComponent.enabled = true; // 🔹 Re-enable dragging after animation completes
            dragComponent.EnableSelfHighlight(true);
        }
    }
}
