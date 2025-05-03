using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Sublimation_SimulationManager : MonoBehaviour
{
    #region Attributes

    [Header("Refrennces")]
    public GameObject[] PickableObjects;
    public BoxCollider[] colliders;
    public Animator[] animators;
    public int step;
    public static Sublimation_SimulationManager ins;

    [Header("Cursor")]
    public CursorMode cursorMode = CursorMode.Auto;
    public Vector2 cursorOffset;
    public Texture2D handCursor;

    [Header("Clamp")]
    float[] zClamp;
    float[] yClamp;
    [SerializeField]
    GameObject lampFire, gb;

    [Header("Animation Events")]
    public GameObject rui;
    public Material finalWhiteThing;
    public GameObject stopwatch;
    public TextMeshProUGUI timer;

    [Header("thermometer")]
    public Color color;
    public Color color1;
    public GameObject funnelData;

    #endregion


    #region Monobehaviour Functions
    void Awake()
    {
        ins = this;
        //adding triggering script to colliding objs
        foreach (BoxCollider box in colliders)
        {
            SublimationColliderBridge cb = box.gameObject.AddComponent<SublimationColliderBridge>();
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
        zClamp = new float[PickableObjects.Length];
        yClamp = new float[PickableObjects.Length];

        for (int i = 0; i < zClamp.Length; i++)
        {
            zClamp[i] = PickableObjects[i].transform.localPosition.z;
        }

        for (int i = 0; i < yClamp.Length; i++)
        {
            yClamp[i] = PickableObjects[i].transform.localPosition.y;
        }

        finalWhiteThing.color = new Color(finalWhiteThing.color.r, finalWhiteThing.color.g, finalWhiteThing.color.b, 0);
        colliders[5].gameObject.SetActive(false);
        stopwatch.SetActive(false);
        funnelData.SetActive(false);

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
                Destroy(PickableObjects[step].GetComponent<SublimationObjectPicker>());
                //      Cursor.SetCursor(null, Vector2.zero, SimulationManager.ins.cursorMode);
                StartCoroutine(turnOffAnimator(animators[step], 2.5f));

                StartCoroutine(NextStepsProcedures());
                break;
            case 1:
                // lampFire.SetActive(true);
                EnableAndPlayAnimation();
                PickableObjects[step].GetComponent<BoxCollider>().enabled = false;
                StartCoroutine(NextStepsProcedures());
                break;
            case 2:
                EnableAndPlayAnimation();
                PickableObjects[step].GetComponent<BoxCollider>().enabled = false;
                StartCoroutine(turnOffAnimator(animators[step], 2.1f));
                StartCoroutine(NextStepsProcedures());
                break;
            case 3:
                EnableAndPlayAnimation();
                colliders[step].gameObject.SetActive(false);
                PickableObjects[step].GetComponent<BoxCollider>().enabled = false;
                StartCoroutine(NextStepsProcedures());

                break;
            case 4:
                EnableAndPlayAnimation();

                PickableObjects[step].GetComponent<BoxCollider>().enabled = false;
                StartCoroutine(NextStepsProcedures());
                //PickableObjects[5].GetComponent<BurnerClick>().enabled = true;
                colliders[5].gameObject.SetActive(true);
                break;
            case 5:
                //animators[step].enabled = true;
                //colliders[step].enabled = false;
                //   animators[step].Play("WireGuazeSit");
                EnableAndPlayAnimation();
                Destroy(PickableObjects[step].GetComponent<SublimationObjectPicker>());

                PickableObjects[step].GetComponent<BoxCollider>().enabled = false;
                StartCoroutine(turnOffAnimator(animators[step], 2.1f));
                StartCoroutine(NextStepsProcedures());


                break;
            case 6:

                lampFire.SetActive(true);
                Cursor.SetCursor(null, Vector2.zero, Sublimation_SimulationManager.ins.cursorMode);
                PickableObjects[step].SetActive(false);
                PickableObjects[step].GetComponent<BoxCollider>().enabled = false;
                //StartCoroutine(NextStepsProcedures());
                Invoke(nameof(CamAnimation), 2);
                break;
            case 7:
                EnableAndPlayAnimation();
                PickableObjects[step].GetComponent<BoxCollider>().enabled = false;
                //StartCoroutine(NextStepsProcedures());
                break;
            case 8:

                break;
        }

    }
    //agar auto chale to matalb collider kahee hit horaha he

    private IEnumerator NextStepsProcedures()
    {
        PickableObjects[step].GetComponent<SublimationObjectPicker>().enabled = false;
        Cursor.SetCursor(null, Vector2.zero, Sublimation_SimulationManager.ins.cursorMode);
        yield return new WaitForSecondsRealtime(1f);
        step++;
        colliders[step].enabled = true;
        Debug.Log("hiss");
        PickableObjects[step].AddComponent<SublimationObjectPicker>();
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
        print("a");
        animators[step].enabled = true;
        colliders[step].gameObject.SetActive(false);
        animators[step].SetTrigger("Play");

    }

    public void CamAnimation()
    {
        PickableObjects[step].GetComponent<SublimationObjectPicker>().enabled = false;
        Camera.main.GetComponent<Animator>().Play("Sublimation_CameraZoom");
    }

    public IEnumerator IEActivateWhiteMaterial()
    {
        funnelData.SetActive(true);
        //finalWhiteThing.color = new Color(finalWhiteThing.color.r, finalWhiteThing.color.g, finalWhiteThing.color.b, 0);
        finalWhiteThing.color = color;

        float t = 0;

        while (t < 1)
        {
            t += Time.deltaTime/1000;

            finalWhiteThing.color = Color.Lerp(finalWhiteThing.color, color1, t);

            yield return null;
        }


        //float t = 4;

        //while (t>0)
        //{
        //    t -= Time.deltaTime;

        //    if(finalWhiteThing.color.a < 1)
        //    {
        //        print("adding value and value =" + finalWhiteThing.color.a);
        //        finalWhiteThing.color = new Color(finalWhiteThing.color.r, finalWhiteThing.color.g, finalWhiteThing.color.b, finalWhiteThing.color.a + Time.deltaTime/2);
        //    }


        yield return null;
    }

}

