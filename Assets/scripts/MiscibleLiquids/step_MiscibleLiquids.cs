using UnityEngine;

public class step_MiscibleLiquids : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("trigger"))
        {
            Debug.Log(other.gameObject.name);
            MiscibleLiquids.instance.TriggerTask();
        }

    }
}
