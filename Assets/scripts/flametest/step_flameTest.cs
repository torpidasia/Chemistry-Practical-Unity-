using UnityEngine;

public class step_flameTest : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("trigger"))
        {
            Debug.Log("Object reached target: " + other.gameObject.name);
            FlameTest.instance.ActivateNextTask();
        }
    }
}
