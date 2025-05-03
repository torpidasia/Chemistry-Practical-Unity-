using UnityEngine;

public class Ethyl_Step : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("trigger"))
        {
            Debug.Log(other.gameObject.name);

            // ✅ Ensure EthylAlcohol has TriggerTask method
            EthylAlcohol.instance?.TriggerTask();
        }
    }
}
