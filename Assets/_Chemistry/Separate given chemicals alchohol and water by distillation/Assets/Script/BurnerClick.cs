using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BurnerClick : MonoBehaviour
{
    public GameObject thermometer;
    public GameObject finalMixture;
    public Animator anim;
    public Image term;
    public Text temprature;

    private void OnMouseDown()
    {
        transform.GetChild(0).gameObject.SetActive(true);
        SimulationManager.ins.OnTriggerEnter(new Collider());

        anim.Play("SpiralTube1");

        Invoke(nameof(ActivateThermometer), 2);
    }

    void ActivateThermometer()
    {
        thermometer.SetActive(true);
        StartCoroutine(IEThermometer());
    }

    IEnumerator IEThermometer()
    {
        term.fillAmount = 0.352f;

        while(term.fillAmount <0.79f)
        {
            term.fillAmount += Time.deltaTime ;

            yield return new WaitForSeconds(0.5f);

            if (term.fillAmount > 0.5F)
                finalMixture.SetActive(true);

            float convertedTemp = (int)(term.fillAmount * 100);

            if(convertedTemp <= 78)
                temprature.text = convertedTemp.ToString() + "°C";

            yield return null;
        }

        //after 78C
        yield return new WaitForSeconds(1.5f);

        Camera.main.GetComponent<Animator>().GetComponent<Animator>().enabled = true;
        Camera.main.GetComponent<Animator>().Play("CameraAnimation");


    }



}
