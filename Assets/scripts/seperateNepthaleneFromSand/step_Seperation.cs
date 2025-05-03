using UnityEngine;

public class step_Seperation : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("trigger"))
        {
            Debug.Log(other.gameObject.name);
            if (seperateNepthaleneFromSand.instance != null)
            {
                seperateNepthaleneFromSand.instance.ActivateNextTask();
            }
        }
    }
}
