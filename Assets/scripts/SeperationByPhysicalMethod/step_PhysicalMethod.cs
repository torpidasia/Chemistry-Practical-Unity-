using UnityEngine;

public class step_PhysicalMethod : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("trigger"))
        {
            Debug.Log(other.gameObject.name);
            SeperationByPhysicalMethod.instance.ActivateNextTask(); // 🔥 Use ActivateNextTask()
        }
    }
}
