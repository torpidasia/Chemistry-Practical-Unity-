using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEvents : MonoBehaviour
{
    [SerializeField]
    GameObject halfFullWater, FullWater, roundFlaskWater, funnel;
    [SerializeField]
    Animator roundFlask;

    public void ActivateHalfFullWater()
    {
        halfFullWater.SetActive(true);
    }
    public void ActivateFullWater()
    {
        FullWater.SetActive(true);
        halfFullWater.SetActive(false);
        Debug.Log("|fullwater");

    }
    public void ActivateRoundFlaskWater()
    {
        roundFlaskWater.SetActive(true);
    }
    public void RoundFlaskAnimation()
    {
        roundFlask.Play("RoundFlaskWaterIncrease");
    }
    public void PutFunnelBack()
    {
        funnel.GetComponent<Animator>().enabled = true;
        funnel.GetComponent<Animator>().Play("Funnel back to position");
        
    }
}
