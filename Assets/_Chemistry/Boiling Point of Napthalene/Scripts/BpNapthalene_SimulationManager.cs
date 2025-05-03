using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BpNapthalene_SimulationManager : MonoBehaviour
{
    #region Attributes

    [Header("Refrennces")]
    public GameObject[] PickableObjects;
    public BoxCollider[] colliders;
    public Animator[] animators;
    public int step;
    public static BpNapthalene_SimulationManager ins;

    [Header("Cursor")]
    public CursorMode cursorMode = CursorMode.Auto;
    public Vector2 cursorOffset;
    public Texture2D handCursor;

    [Header("Clamp")]
    float[] zClamp;
    float[] yClamp;
    [SerializeField]
    GameObject gb;

    [Header("Lamp Settings")]
    public Material[] material;
    public MeshRenderer[] fireTopi;


    [Header("Animation Events")]
    public GameObject firstFire;
    public GameObject secondFire;
    public GameObject lampFire, CapilariesInnerData, rabbat, orgRubber, thermomentersCapilarytube, stirrer, mercury;
    public Animator mercuryAnimator;
    public Animator matchBoxAnimator;

    [Header("thermometer")]
    public GameObject thermometerCanvas;
    public Text temprature;
    public Image term;

    #endregion


    #region Monobehaviour Functions
    void Awake()
    {
        ins = this;
        //adding triggering script to colliding objs
        foreach (BoxCollider box in colliders)
        {
            BpNapthaleneColliderBridge cb = box.gameObject.AddComponent<BpNapthaleneColliderBridge>();
            cb.Initialize(this);
        }

        //disabling all animators initially
        foreach (Animator anim in animators)
            anim.enabled = false;

        //disabling all colliders initially
        foreach (BoxCollider collider in colliders)
        {
            collider.enabled = false;
            collider.isTrigger = true;
        }

    }


    private void Start()
    {
        step = 0;
        colliders[step].enabled = true;
        //      PickableObjects[5].GetComponent<BurnerClick>().enabled = false;
        //PickableObjects = null;
        zClamp = new float[PickableObjects.Length];
        yClamp = new float[PickableObjects.Length];
        //PickableObjects[10].GetComponent<BoxCollider>().enabled = false;

        for (int i = 0; i < zClamp.Length; i++)
        {
            zClamp[i] = PickableObjects[i].transform.localPosition.z;
        }

        for (int i = 0; i < yClamp.Length; i++)
        {
            yClamp[i] = PickableObjects[i].transform.localPosition.y;
        }

        thermometerCanvas.SetActive(false);
    }


    private void Update()
    {
        //for (int i = 0; i < PickableObjects.Length; i++)
        //{

        //    PickableObjects[i].transform.localPosition = new Vector3(PickableObjects[i].transform.localPosition.x, PickableObjects[i].transform.localPosition.y, zClamp[i]);

        //    if (PickableObjects[i].transform.localPosition.y < yClamp[i])
        //        PickableObjects[i].transform.localPosition = new Vector3(PickableObjects[i].transform.localPosition.x, yClamp[i], PickableObjects[i].transform.localPosition.z);
        //}
        //  Debug.Log(x);
    }



    public void OnCollisionEnter(Collision collision)
    {
        // Do your stuff here
    }

    public void OnTriggerEnter(Collider other)
    {
        Debug.Log("hi");
        switch (step)
        {
            case 0:
                EnableAndPlayAnimation();
                PickableObjects[step].GetComponent<BoxCollider>().enabled = false;
                Destroy(PickableObjects[step].GetComponent<BoilingPointNapthelene_ObjectPicker>());
                //      Cursor.SetCursor(null, Vector2.zero, SimulationManager.ins.cursorMode);
                StartCoroutine(turnOffAnimator(animators[step], 2.1f));

                StartCoroutine(NextStepsProcedures());
                break;
            case 1:
                if (other.tag == "Match")
                {
                    fireTopi[0].material = material[1];
                    lampFire.SetActive(true);
                    PickableObjects[step].SetActive(false);
                    animators[step].enabled = true;
                    colliders[step].enabled = false;
                    PickableObjects[step].GetComponent<BoxCollider>().enabled = false;
                    animators[0].enabled = false;
                    StartCoroutine(NextStepsProcedures());
                }
                break;
            case 2:
                if (other.tag == "CappilaryTube")
                {
                    animators[step].enabled = true;
                    colliders[step].enabled = false;

                    PickableObjects[step].GetComponent<BoxCollider>().enabled = false;
                    StartCoroutine(turnOffAnimator(animators[step], 2.1f));
                    Destroy(PickableObjects[step].GetComponent<BoilingPointNapthelene_ObjectPicker>());

                    StartCoroutine(NextStepsProcedures());
                }
                break;
            case 3:
                if (other.tag == "CappilaryTube")
                {
                    animators[step].enabled = true;
                    animators[step].Play("cappilarry tube to original position");
                    StartCoroutine(turnOffAnimator(animators[step], 1.1f));
                    Destroy(PickableObjects[step].GetComponent<BoilingPointNapthelene_ObjectPicker>());
                    colliders[step].enabled = false;
                    PickableObjects[step].GetComponent<BoxCollider>().enabled = false;
                    StartCoroutine(NextStepsProcedures());
                }
                break;
            case 4:
                if (other.tag == "cap")
                {
                    fireTopi[0].material = material[0];
                    animators[step].enabled = true;
                    colliders[step].enabled = false;
                    PickableObjects[step].GetComponent<BoxCollider>().enabled = false;
                    StartCoroutine(NextStepsProcedures());
                }
                break;
            case 5:
                animators[step].enabled = true;
                colliders[step].enabled = false;
                PickableObjects[step].GetComponent<BoxCollider>().enabled = false;
                StartCoroutine(NextStepsProcedures());
                break;
            case 6:
                animators[step].enabled = true;
                colliders[step].enabled = false;
                animators[step].Play("capilarrytubetiewiththewrma");
                PickableObjects[step].GetComponent<BoxCollider>().enabled = false;
                StartCoroutine(NextStepsProcedures());
                //PickableObjects[5].GetComponent<BurnerClick>().enabled = true;

                break;
            case 7:                                                         //rubber band
                if (other.tag == "RubberBand")
                {
                    animators[step].enabled = true;
                    colliders[step].enabled = false;
                    animators[step].Play("rabbatkotmareh");
                    PickableObjects[step].GetComponent<BoxCollider>().enabled = false;
                    StartCoroutine(turnOffAnimator(animators[step], 1.1f));
                    StartCoroutine(NextStepsProcedures());
                    colliders[8].gameObject.SetActive(true);
                }
                break;
            case 8:
                animators[step].enabled = true;
                colliders[step].enabled = false;
                animators[step].Play("WireGuazeSit");
                print("abc");
                PickableObjects[step].GetComponent<BoxCollider>().enabled = false;
                StartCoroutine(turnOffAnimator(animators[step], 1.1f));
                StartCoroutine(NextStepsProcedures());
                colliders[9].gameObject.SetActive(true);
                break;
            case 9:
                PickableObjects[1].SetActive(true);
                matchBoxAnimator.enabled = true;
                matchBoxAnimator.Play("New State");
                EnableAndPlayAnimation();
                PickableObjects[step].GetComponent<BoxCollider>().enabled = false;
                StartCoroutine(turnOffAnimator(animators[step], 2.1f));
                StartCoroutine(NextStepsProcedures());
                break;
            case 10:
                fireTopi[1].material = material[1];
                PickableObjects[1].SetActive(false);
                StartCoroutine(NextStepsProcedures());
                secondFire.SetActive(true);
                break;
            case 11:
                EnableAndPlayAnimation();
                PickableObjects[step].GetComponent<BoxCollider>().enabled = false;
                StartCoroutine(NextStepsProcedures());
                break;
            case 12:
                EnableAndPlayAnimation();
                PickableObjects[step].GetComponent<BoxCollider>().enabled = false;
                StartCoroutine(NextStepsProcedures());
                break;
            case 13:
                if (other.tag == "Stirrer")
                {
                    EnableAndPlayAnimation();
                    //animators[step].Play("Stirring");
                    PickableObjects[step].GetComponent<BoxCollider>().enabled = false;
                    Invoke(nameof(ActiveThermometerCanvas), 2);
                }
                break;
        }
    }
    //agar auto chale to matalb collider kahee hit horaha he

    private IEnumerator NextStepsProcedures()
    {
        PickableObjects[step].GetComponent<BoilingPointNapthelene_ObjectPicker>().enabled = false;
        Cursor.SetCursor(null, Vector2.zero, BpNapthalene_SimulationManager.ins.cursorMode);
        yield return new WaitForSecondsRealtime(1f);
        step++;
        colliders[step].enabled = true;
        Debug.Log("hiss");
        PickableObjects[step].AddComponent<BoilingPointNapthelene_ObjectPicker>();
        Debug.Log("asdad");
        PickableObjects[step].GetComponent<BoxCollider>().enabled = true;
    }


    #endregion

    private IEnumerator turnOffAnimator(Animator anim, float x)
    {
        yield return new WaitForSecondsRealtime(x);
        anim.enabled = false;
    }
    public void ResetButton()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);
    }

    private void EnableAndPlayAnimation()
    {
        animators[step].enabled = true;
        colliders[step].gameObject.SetActive(false);
        animators[step].SetTrigger("Play");

    }

    public void ActiveThermometerCanvas()
    {
        //stirrer.GetComponent<Animator>().Play("Stirring");
        thermometerCanvas.SetActive(true);
        temprature.text = "30°C";
        StartCoroutine(IEThermometerValuie());
        Camera.main.GetComponent<Animator>().Play("BPNAP_CameraZoom");

    }

    IEnumerator IEThermometerValuie()
    {
        term.fillAmount = 0.30f;
        while (term.fillAmount < 0.79f)
        {
            //term.fillAmount += Time.deltaTime * 1.5f;
            term.fillAmount += 0.01f;

            yield return new WaitForSeconds(1f);

            float convertedTemp = (int)(term.fillAmount * 100);

            if (convertedTemp <= 81)
                temprature.text = convertedTemp.ToString() + "°C";

            yield return null;
        }

        //after 81C
        yield return new WaitForSeconds(1.5f);
        temprature.text = "81°C";
        stirrer.SetActive(false);
        mercuryAnimator.enabled = false;

    }
}

