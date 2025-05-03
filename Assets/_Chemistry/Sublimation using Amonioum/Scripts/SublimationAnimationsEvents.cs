using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SublimationAnimationsEvents : MonoBehaviour
{
    public void RuiOn()
    {
        Sublimation_SimulationManager.ins.rui.SetActive(true);
    }

    void StartStopWatch()
    {
        Sublimation_SimulationManager.ins.stopwatch.SetActive(true);
        Sublimation_SimulationManager.ins.stopwatch.GetComponent<Animator>().enabled = true;
        Sublimation_SimulationManager.ins.stopwatch.GetComponent<Animator>().Play("Sublimation_TimeTick");
        StartCoroutine(IEStopWatchTimer());
    }

    IEnumerator IEStopWatchTimer()
    {
        float time = 15;
        Sublimation_SimulationManager.ins.timer.text = time.ToString();

        while (time > 0)
        {
            yield return new WaitForSecondsRealtime(1);
            time--;
            Sublimation_SimulationManager.ins.timer.text = time.ToString();

            yield return null;
        }

        Sublimation_SimulationManager.ins.stopwatch.SetActive(false);
        StartCoroutine(Sublimation_SimulationManager.ins.IEActivateWhiteMaterial());
    }

}
