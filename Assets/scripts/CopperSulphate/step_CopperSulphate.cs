using UnityEngine;

public class step_CopperSulphate : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
     
        if (other.CompareTag("trigger"))
        {
            Debug.Log("Collided with: " + other.gameObject.name);
            CopperSulphate.instance.TriggerTask();
        }

    }
}
