using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BpNapthaleneAnimEvent : MonoBehaviour
{
    public void TurnNapthaleneInnerDataOn()
    {
        BpNapthalene_SimulationManager.ins.CapilariesInnerData.SetActive(true);
    }

    public void RabbatObjanim()
    {
        BpNapthalene_SimulationManager.ins.rabbat.GetComponent<Animator>().Play("rabbatkotmareh");
    }

    public void TurnRubberOn()
    {
        BpNapthalene_SimulationManager.ins.orgRubber.SetActive(true);
    }

    public void TurnTmCapOn()
    {
        BpNapthalene_SimulationManager.ins.thermomentersCapilarytube.SetActive(true);
    }

    public void FinalThermoAnimation()
    {
        BpNapthalene_SimulationManager.ins.mercury.SetActive(true);
        BpNapthalene_SimulationManager.ins.mercuryAnimator.enabled = true;
        BpNapthalene_SimulationManager.ins.mercuryAnimator.Play("MercuryAnimation1");
    }

    public void ExtinguishFirstFire()
    {
        BpNapthalene_SimulationManager.ins.firstFire.SetActive(false);
    }

}
