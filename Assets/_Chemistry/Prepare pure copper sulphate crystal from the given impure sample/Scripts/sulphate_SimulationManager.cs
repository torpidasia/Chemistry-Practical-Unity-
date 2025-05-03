using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class sulphate_SimulationManager : MonoBehaviour
{
    #region Attributes

    [Header("Refrennces")]
    public GameObject[] PickableObjects;
    public BoxCollider[] colliders;
    public Animator[] animators;
    public int step;
    public static sulphate_SimulationManager ins;

    [Header("Cursor")]
    public Texture2D handCursor;
    public CursorMode cursorMode = CursorMode.Auto;
    public Vector2 cursorOffset;

    [Header("Clamp")]
    float[] zClamp;
    float[] yClamp;
    public bool clamping;


    [Header("Animation Events")]
    public MeshRenderer water;
    public Color waterWithSulphate;
    public GameObject sulphateInWater;
    public GameObject funnelData;
    public GameObject firstFilterPaper;
    public GameObject blueDrops;
    public GameObject flame;
    public GameObject pureSUlphateInFilterPaper;
    public Animator sitterAnim;
    public Animator funnelAnimator;
    public Animator stopWatchAnimator;
    public Animator filterPaperAnimator;

    [Header("Timer")]
    public GameObject endingCanvas;
    public GameObject stopWatch;
    public TextMeshProUGUI stopWatchText;
    public AudioClip igniteSound;
    public AudioClip dropObjectWater;
    public AudioClip mixing;
    public AudioSource stopWatchAS;


    #endregion

    

    #region Monobehaviour Functions
    void Awake()
    {
        ins = this;
        //adding triggering script to colliding objs
        foreach (BoxCollider box in colliders)
        {
            sulphateColliderBridge cb = box.gameObject.AddComponent<sulphateColliderBridge>();
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

        colliders[12].gameObject.SetActive(false);

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

        sulphateInWater.SetActive(false);
        funnelData.SetActive(false);
        stopWatch.SetActive(false);
        pureSUlphateInFilterPaper.SetActive(false);

        clamping = true;
    }


    private void Update()
    {
        if (clamping)
        {
            for (int i = 0; i < PickableObjects.Length; i++)
            {

                //PickableObjects[i].transform.localPosition = new Vector3(PickableObjects[i].transform.localPosition.x, PickableObjects[i].transform.localPosition.y, zClamp[i]);

                //if (PickableObjects[i].transform.localPosition.y < yClamp[i])
                //    PickableObjects[i].transform.localPosition = new Vector3(PickableObjects[i].transform.localPosition.x, yClamp[i], PickableObjects[i].transform.localPosition.z);
            }
        }
    }



    public void OnCollisionEnter(Collision collision)
    {
        // Do your stuff here
    }

    public void OnTriggerEnter(Collider other)
    {
        //Debug.Log("hi");
        switch (step)
        {
            case 0:
                if (other.transform.name == "CuSO4Cap")
                {
                    print("aaa");
                    clamping = false;
                    EnableAndPlayAnimation();
                    PickableObjects[step].GetComponent<BoxCollider>().enabled = false;
                    Destroy(PickableObjects[step].GetComponent<sulphateObjectPicker>());
                    //      Cursor.SetCursor(null, Vector2.zero, SimulationManager.ins.cursorMode);
                    StartCoroutine(turnOffAnimator(animators[step], 2.5f));

                    StartCoroutine(NextStepsProcedures());
                }
                break;

            case 1:
                if (other.tag == "Spoon")
                {
                    clamping = false;
                    EnableAndPlayAnimation();
                    PickableObjects[step].GetComponent<BoxCollider>().enabled = false;
                    Destroy(PickableObjects[step].GetComponent<sulphateObjectPicker>());
                    //      Cursor.SetCursor(null, Vector2.zero, SimulationManager.ins.cursorMode);
                    StartCoroutine(turnOffAnimator(animators[step], 2.5f));

                    StartCoroutine(NextStepsProcedures());
                }
                break;
            case 2:
                if (other.tag == "Spoon")
                {
                    EnableAndPlayAnimation();
                    PickableObjects[step].GetComponent<BoxCollider>().enabled = false;
                    Destroy(PickableObjects[step].GetComponent<sulphateObjectPicker>());
                    Cursor.SetCursor(null, Vector2.zero, cursorMode);
                    animators[step].Play("Sulphate in Water");
                    StartCoroutine(turnOffAnimator(animators[step], 2.5f));

                    StartCoroutine(NextStepsProcedures());
                }
                break;

            case 3:
                if (other.tag == "Stirrer")
                {
                    clamping = false;
                    EnableAndPlayAnimation();
                    PickableObjects[step].GetComponent<BoxCollider>().enabled = false;
                    Destroy(PickableObjects[step].GetComponent<sulphateObjectPicker>());
                    Cursor.SetCursor(null, Vector2.zero, cursorMode);
                    animators[step].Play("Sulphate in Water");
                    //StartCoroutine(turnOffAnimator(animators[step], 2.5f));

                    StartCoroutine(NextStepsProcedures());
                    //CuSo4 cap on org position
                }
                break;

            case 4:
                if (other.transform.name == "CuSO4Cap")
                {
                    print("other.transform.name ==");
                    clamping = false;
                    EnableAndPlayAnimation();
                    animators[step].Play("CuSo4 cap on org position");
                    PickableObjects[step].GetComponent<BoxCollider>().enabled = false;
                    Destroy(PickableObjects[step].GetComponent<sulphateObjectPicker>());
                    //      Cursor.SetCursor(null, Vector2.zero, SimulationManager.ins.cursorMode);
                    StartCoroutine(turnOffAnimator(animators[step], 2.5f));

                    StartCoroutine(NextStepsProcedures());
                }
                break;

            case 5:
                if (other.tag == "Funnel")
                {
                    EnableAndPlayAnimation();
                    animators[step].Play("FunnelOnTopofGauze");
                    PickableObjects[step].GetComponent<BoxCollider>().enabled = false;
                    Destroy(PickableObjects[step].GetComponent<sulphateObjectPicker>());
                    Cursor.SetCursor(null, Vector2.zero, cursorMode);
                    StartCoroutine(turnOffAnimator(animators[step], 2.5f));

                    StartCoroutine(NextStepsProcedures());
                }
                break;


            case 6:
                if (other.tag == "China Dish")
                {
                    EnableAndPlayAnimation();
                    PickableObjects[step].GetComponent<BoxCollider>().enabled = false;
                    Destroy(PickableObjects[step].GetComponent<sulphateObjectPicker>());
                    Cursor.SetCursor(null, Vector2.zero, cursorMode);
                    StartCoroutine(turnOffAnimator(animators[step], 2.5f));

                    StartCoroutine(NextStepsProcedures());
                }
                break;

            case 7:
                if (other.tag == "Filter Paper")
                {
                    EnableAndPlayAnimation();
                    PickableObjects[step].GetComponent<BoxCollider>().enabled = false;
                    Destroy(PickableObjects[step].GetComponent<sulphateObjectPicker>());
                    Cursor.SetCursor(null, Vector2.zero, cursorMode);
                    StartCoroutine(turnOffAnimator(animators[step], 2.5f));

                    StartCoroutine(NextStepsProcedures());

                }
                break;

            case 8:
                if (other.tag == "Stirrer")
                {
                    clamping = false;
                    EnableAndPlayAnimation();
                    animators[step].Play("StirrerToFunnelTop");
                    PickableObjects[step].GetComponent<BoxCollider>().enabled = false;
                    Destroy(PickableObjects[step].GetComponent<sulphateObjectPicker>());
                    Cursor.SetCursor(null, Vector2.zero, cursorMode);
                    animators[step].Play("Sulphate in Water");
                    //StartCoroutine(turnOffAnimator(animators[step], 2.5f));

                    StartCoroutine(NextStepsProcedures());
                }
                break;


            case 9:
                if (other.tag == "Beaker")
                {
                    EnableAndPlayAnimation();
                    PickableObjects[step].GetComponent<BoxCollider>().enabled = false;
                    Destroy(PickableObjects[step].GetComponent<sulphateObjectPicker>());
                    Cursor.SetCursor(null, Vector2.zero, cursorMode);
                    //StartCoroutine(turnOffAnimator(animators[step], 2.5f));

                    //StartCoroutine(NextStepsProcedures());
                }
                break;

            case 10:
                EnableAndPlayAnimation();
                animators[step].Play("funnelToOriginalPos");
                PickableObjects[step].GetComponent<BoxCollider>().enabled = false;
                Destroy(PickableObjects[step].GetComponent<sulphateObjectPicker>());
                Cursor.SetCursor(null, Vector2.zero, cursorMode);
                StartCoroutine(turnOffAnimator(animators[step], 2.5f));

                StartCoroutine(NextStepsProcedures());

                break;

            case 11:
                if (other.tag == "WireGauze")
                {
                    EnableAndPlayAnimation();
                    PickableObjects[step].GetComponent<BoxCollider>().enabled = false;
                    Destroy(PickableObjects[step].GetComponent<sulphateObjectPicker>());
                    Cursor.SetCursor(null, Vector2.zero, cursorMode);
                    StartCoroutine(turnOffAnimator(animators[step], 2.5f));

                    StartCoroutine(NextStepsProcedures());
                }
                break;

            case 12:
                if (other.tag == "China Dish")
                {
                    EnableAndPlayAnimation();
                    animators[step].Play("DishOnTopOfWireGuaze");
                    PickableObjects[step].GetComponent<BoxCollider>().enabled = false;
                    Destroy(PickableObjects[step].GetComponent<sulphateObjectPicker>());
                    Cursor.SetCursor(null, Vector2.zero, cursorMode);
                    //StartCoroutine(turnOffAnimator(animators[step], 2.5f));

                    StartCoroutine(NextStepsProcedures());
                }
                break;

            case 13:
                if (other.tag == "Lamp")
                {
                    EnableAndPlayAnimation();
                    PickableObjects[step].GetComponent<BoxCollider>().enabled = false;
                    Destroy(PickableObjects[step].GetComponent<sulphateObjectPicker>());
                    Cursor.SetCursor(null, Vector2.zero, cursorMode);
                    StartCoroutine(turnOffAnimator(animators[step], 2.5f));
                    colliders[11].gameObject.SetActive(true);


                    StartCoroutine(NextStepsProcedures());
                }
                break;

            case 14:
                if (other.tag == "Match")
                {
                    EnableAndPlayAnimation();
                    PickableObjects[step].GetComponent<BoxCollider>().enabled = false;
                    Destroy(PickableObjects[step].GetComponent<sulphateObjectPicker>());
                    Cursor.SetCursor(null, Vector2.zero, cursorMode);
                    StartCoroutine(turnOffAnimator(animators[step], 2.5f));

                    StartCoroutine(NextStepsProcedures());
                }
                break;

            case 15:
                if (other.tag == "Match")
                {
                    flame.SetActive(true);
                    PickableObjects[step].SetActive(false);
                    PickableObjects[step].GetComponent<BoxCollider>().enabled = false;
                    Destroy(PickableObjects[step].GetComponent<sulphateObjectPicker>());
                    Cursor.SetCursor(null, Vector2.zero, cursorMode);


                    StartCoroutine(StartStopWatch());
                }
                break;

            case 16:
                if (other.tag == "cap")
                {
                    clamping = false;
                    EnableAndPlayAnimation();

                    PickableObjects[step].GetComponent<BoxCollider>().enabled = false;
                    Destroy(PickableObjects[step].GetComponent<sulphateObjectPicker>());
                    Cursor.SetCursor(null, Vector2.zero, cursorMode);


                    StartCoroutine(NextStepsProcedures());
                }
                break;

            case 17:
                if (other.tag == "Spoon")
                {
                    EnableAndPlayAnimation();
                    animators[step].Play("PickPureSulPhateFromDish");
                    PickableObjects[step].GetComponent<BoxCollider>().enabled = false;
                    Destroy(PickableObjects[step].GetComponent<sulphateObjectPicker>());
                    Cursor.SetCursor(null, Vector2.zero, cursorMode);
                    StartCoroutine(turnOffAnimator(animators[step], 2.5f));

                    StartCoroutine(NextStepsProcedures());
                }
                break;

            case 18:
                if (other.tag == "Spoon")
                {
                    EnableAndPlayAnimation();
                    animators[step].Play("PutSulPhateOnFilterPaper");
                    PickableObjects[step].GetComponent<BoxCollider>().enabled = false;
                    Destroy(PickableObjects[step].GetComponent<sulphateObjectPicker>());
                    Cursor.SetCursor(null, Vector2.zero, cursorMode);
                    StartCoroutine(turnOffAnimator(animators[step], 2.5f));

                    Invoke(nameof(CamAnimation), 3);
                }
                break;


        }

    }
    //agar auto chale to matalb collider kahee hit horaha he

    public IEnumerator NextStepsProcedures()
    {
        //PickableObjects[step].GetComponent<sulphateObjectPicker>().enabled = false;
        Cursor.SetCursor(null, Vector2.zero, cursorMode);
        yield return new WaitForSecondsRealtime(1f);
        step++;
        colliders[step].enabled = true;
        Debug.Log("NextStepsProcedures ");
        PickableObjects[step].AddComponent<sulphateObjectPicker>();
        PickableObjects[step].GetComponent<BoxCollider>().enabled = true;
    }


    #endregion

    public IEnumerator turnOffAnimator(Animator anim, float x)
    {
        clamping = true;
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

    public void CamAnimation()
    {
        Camera.main.GetComponent<Animator>().Play("CamZoom");
    }

    IEnumerator StartStopWatch()
    {
        float time = 15;

        print("StartStopWatch");

        stopWatch.SetActive(true);
        stopWatchText.text = time.ToString();
        stopWatchAnimator.enabled = true;
        stopWatchAnimator.Play("StopWathAnimation 0");
        stopWatchAnimator.speed = 1;
        stopWatchAnimator.speed = stopWatchAnimator.speed / time;
        stopWatchAS.Play();

        while (time > 0)
        {
            yield return new WaitForSecondsRealtime(1);
            time--;
            stopWatchText.text = time.ToString();
        }

        stopWatchAS.Stop();

        blueDrops.SetActive(false);
        stopWatchText.text = "0";
        stopWatch.SetActive(false);
        funnelAnimator.enabled = false;

        StartCoroutine(NextStepsProcedures());
    }

    public void ChangeCapClampPos()
    {
        zClamp[step] = PickableObjects[step].transform.position.z;
        clamping = true;
    }



}

