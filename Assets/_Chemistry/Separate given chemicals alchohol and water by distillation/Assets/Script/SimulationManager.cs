using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimulationManager : MonoBehaviour
{
    #region Attributes

    public GameObject[] PickableObjects;
    public BoxCollider[] colliders;
    public Animator[] animators;
    public int step;
    public static SimulationManager ins;

    [Header("Cursor")]
    public CursorMode cursorMode = CursorMode.Auto;
    public Vector2 cursorOffset;
    public Texture2D handCursor;


    float[] zClamp;
    #endregion

    #region Monobehaviour Functions
    void Awake()
    {
        ins = this;
        //adding triggering script to colliding objs
        foreach (BoxCollider box in colliders)
        {
            ColliderBridge cb = box.gameObject.AddComponent<ColliderBridge>();
            cb.Initialize(this);
        }

        //disabling all animators initially
        foreach(Animator anim in animators)
            anim.enabled = false;

        //disabling all colliders initially
        foreach (BoxCollider collider in colliders)
            collider.enabled = false;


    }

    void Start()
    {
        step = 0;
        colliders[step].enabled = true;
        PickableObjects[7].GetComponent<BurnerClick>().enabled = false;

        zClamp = new float[PickableObjects.Length];

        for(int i =0; i<zClamp.Length; i++)
        {
            zClamp[i] = PickableObjects[i].transform.localPosition.z;
        }
    }


    private void Update()
    {
        for(int i =0; i<PickableObjects.Length; i++)
        {
            PickableObjects[i].transform.localPosition = new Vector3(PickableObjects[i].transform.localPosition.x, PickableObjects[i].transform.localPosition.y, zClamp[i]);

        }
    }



    public void OnCollisionEnter(Collision collision)
    {
        // Do your stuff here
    }

    public void OnTriggerEnter(Collider other)
    {
        switch (step)
        {
            case 0:
                animators[step].enabled = true;
                colliders[step].enabled = false;
                animators[step].Play("Step1Beaker");

                ObjectPicker obj = PickableObjects[step].GetComponent<ObjectPicker>();
                Destroy(obj);
                PickableObjects[step].GetComponent<BoxCollider>().enabled = false;
                Cursor.SetCursor(null, Vector2.zero, SimulationManager.ins.cursorMode);

                StartCoroutine(NextStepsProcedures());
                break;
            case 1:
                animators[step].enabled = true;
                colliders[step].enabled = false; 
                animators[step].Play("Step2Water");
                PickableObjects[step].GetComponent<BoxCollider>().enabled = false;
                animators[0].enabled = false;
                StartCoroutine(NextStepsProcedures());
                break;
            case 2:
                animators[step].enabled = true;
                colliders[step].enabled = false;
                animators[step].Play("Step3Water");
                PickableObjects[step].GetComponent<BoxCollider>().enabled = false;
                StartCoroutine( NextStepsProcedures());
                break;
            case 3:
                animators[step].enabled = true;
                colliders[step].enabled = false;
                animators[step].Play("Step4Funnel");
                PickableObjects[step].GetComponent<BoxCollider>().enabled = false;
                StartCoroutine( NextStepsProcedures());
                break;
            case 4:
                animators[step].enabled = true;
                colliders[step].enabled = false;
                animators[step].Play("Step5BeakerToFunnel");
                PickableObjects[step].GetComponent<BoxCollider>().enabled = false;
                StartCoroutine(NextStepsProcedures());
                

                break;
            case 5:
                animators[step].enabled = true;
                colliders[step].enabled = false;
                animators[step].Play("cork on top");
                PickableObjects[step].GetComponent<BoxCollider>().enabled = false;
                StartCoroutine(NextStepsProcedures());
                break;
            case 6:
                animators[step].enabled = true;
                colliders[step].enabled = false;
                animators[step].Play("thermincork");
                PickableObjects[step].GetComponent<BoxCollider>().enabled = false;
                StartCoroutine(NextStepsProcedures());
                PickableObjects[7].GetComponent<BurnerClick>().enabled = true;
                break;

            case 7:
                animators[step].enabled = true;
                PickableObjects[step].GetComponent<BoxCollider>().enabled = false;
                PickableObjects[step].GetComponent<BurnerClick>().enabled = false;
                step++;
                
                break;
        }
    }

    private IEnumerator NextStepsProcedures()
    {
        PickableObjects[step].GetComponent<ObjectPicker>().enabled = false;
       
        yield return new WaitForSecondsRealtime(2f);
        step++;
        PickableObjects[step].AddComponent<ObjectPicker>();
        colliders[step].enabled = true;
        PickableObjects[step].GetComponent<ObjectPicker>().enabled = true;
    }
    // Start is called before the first frame update
    #endregion
    
    public void ResetButton()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);
    }


}
