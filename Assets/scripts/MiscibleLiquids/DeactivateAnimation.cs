using UnityEngine;

public class DeactivateAnimation : MonoBehaviour
{
    // Start is called before the first frame update
 
    public void DeactivateAnimator()
    {
        GetComponent<Animator>().enabled = false;
    }
}
