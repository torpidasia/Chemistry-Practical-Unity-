using UnityEngine;

public class Step_DecompositionReaction : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("trigger"))
        {
            Debug.Log(other.gameObject.name);

            // ✅ Ensure the object moves to the next task naturally
            DecompositionReaction.instance?.OnObjectDropped(other.gameObject);
        }
    }
}
