using UnityEngine;

public class step_ChemicalMethod : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("trigger"))
        {
            Debug.Log(other.gameObject.name);
            if (SeperationByChemicalMethod.instance != null)
            {
                SeperationByChemicalMethod.instance.ActivateNextTask();
            }
        }
    }
}
