using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sulphateAnimationsEvents : MonoBehaviour
{
    sulphate_SimulationManager sim;

    private void Start()
    {
        sim = sulphate_SimulationManager.ins;
    }

    void ActiveSulphateInWater()
    {
        print("ActiveSulphateInWater");
        sim.sulphateInWater.SetActive(true);
    }

    void StartColorLerp()
    {
        //sim.GetComponent<AudioSource>().PlayOneShot(sim.mixing, 0.3f);
        StartCoroutine(ColorLerp());
        sim.sitterAnim.SetTrigger("play");
    }

    IEnumerator ColorLerp()
    {
        float t = 0;

        while(t<0.01f)
        {
            //print("Lerping = "+t);

            t += (Time.deltaTime/500);

            sim.water.material.color = Color.Lerp(sim.water.material.color, sim.waterWithSulphate, t);

            yield return null;
        }

        sim.sulphateInWater.SetActive(false);
        sim.sitterAnim.enabled = true;
        sim.sitterAnim.Play("StirrerToOriginalPos");


        yield return null;
    }

    void EnableFilterPaper()
    {
        sim.firstFilterPaper.SetActive(false);
        sim.funnelData.SetActive(true);
    }

    void StartDropsInDish()
    {
        sim.funnelAnimator.enabled = true;
        sim.funnelAnimator.Play("DropsinChinaDish");

        StartCoroutine(StartStopWatch());
    }

    IEnumerator StartStopWatch()
    {
        float time = 10;

        sim.stopWatch.SetActive(true);
        sim.stopWatchText.text = time.ToString();
        sim.stopWatchAnimator.speed = sim.stopWatchAnimator.speed / 10;

        sim.stopWatchAS.Play();

        while (time>0)
        {
            yield return new WaitForSecondsRealtime(1);
            time--;
            sim.stopWatchText.text = time.ToString();
        }

        sim.blueDrops.SetActive(false);
        sim.stopWatchText.text = "0";
        sim.stopWatch.SetActive(false); 
        sim.funnelAnimator.enabled = false;

        sim.stopWatchAS.Stop();

        StartCoroutine(sim.turnOffAnimator(sim.animators[sim.step], 0.5f));

        StartCoroutine(sim.NextStepsProcedures());

        yield return null;
    }

    void ChangeClampPosition()
    {
        sim.ChangeCapClampPos();
    }

    void EnablepureSUlphateInFilterPaper()
    {
        sim.pureSUlphateInFilterPaper.SetActive(true);
    }

    void PlayFinalFiltration()
    {
        sim.filterPaperAnimator.Play("SoakingPureSulphate");
    }

    void ToggleClamping()
    {
        sim.clamping = !sim.clamping;
        GameObject.Find("stirrer").GetComponent<Animator>().enabled = false;
        print("animator = " + GameObject.Find("stirrer").GetComponent<Animator>().enabled);
    }

    void TurnOffFlame()
    {
        sim.flame.SetActive(false);
    }

    void PlayIgniteSound(float volume)      //d
    {
        sim.GetComponent<AudioSource>().PlayOneShot(sim.igniteSound, volume);
    }

    void PlayDropWaterSoundSound(float volume)      //d
    {
        sim.GetComponent<AudioSource>().PlayOneShot(sim.dropObjectWater, volume);
    }

     void PlayMixingSoundSound(float volume)    //d
    {
        //print("mixing sound");
        //sim.GetComponent<AudioSource>().PlayOneShot(sim.mixing, volume);
    }

    void EndingCanvasTrue()
    {
        sim.endingCanvas.SetActive(true);
    }

    public void EndingCanvasTrue2()
    {
        Invoke(nameof(EndingCanvasTrue), 3);
    }

    public void StirrerToOrgPosition()
    {
        GameObject.Find("stirrer").GetComponent<Animator>().enabled = true;
        GameObject.Find("stirrer").GetComponent<Animator>().Play("StirrerToOriginalPos");
    }


}
