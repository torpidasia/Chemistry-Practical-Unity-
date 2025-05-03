using UnityEngine;

public class step : MonoBehaviour
{
    public string tag;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(tag))
        {
            Debug.Log(other.gameObject.name);
            Sublimation.instance.ActivateNextTask();
        }
    }
}
