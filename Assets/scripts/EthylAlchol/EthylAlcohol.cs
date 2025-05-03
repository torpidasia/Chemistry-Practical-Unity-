using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class EthylAlcohol : MonoBehaviour
{
    public static EthylAlcohol instance;

    [Header("UI")]
    public GameObject taskCompletePanel;
    public TextMeshProUGUI taskText;        // Shows current task only
    public TextMeshProUGUI taskListText;    // Shows all tasks with bullets
    public GameObject taskPanel;

    [Header("Tasks")]
    public string[] taskString;
    public int currentTask;

    [Header("Scene Objects")]
    public GameObject ethyl, tube, beaker, thermometer, matchBox, fire;
    public GameObject[] colliders;

    void Start()
    {
        Screen.orientation = ScreenOrientation.LandscapeLeft;
        instance = this;
        currentTask = 0;

        SetObjectState(ethyl, true);
        SetObjectState(beaker, false);
        SetObjectState(tube, false);
        SetObjectState(thermometer, false);
        SetObjectState(matchBox, false);

        if (colliders.Length > 0)
            colliders[currentTask].SetActive(true);

        taskText.text = taskString[currentTask];
        UpdateTaskListUI();
    }

    public void TaskComplete()
    {
        taskPanel.SetActive(false);

        taskCompletePanel.SetActive(true);
        Invoke(nameof(LoadMainMenu), 3f);
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void TriggerTask()
    {
        switch (currentTask)
        {
            case 0: Task1(); break;
            case 1: Task2(); break;
            case 2: Task3(); break;
            case 3: Task4(); break;
            case 4: Task5(); break;
        }
    }

    public void Task1()
    {
        AdvanceTask();
        Animate(ethyl);
        SetObjectState(beaker, true);
    }

    public void Task2()
    {
        AdvanceTask();
        Animate(beaker);
        SetObjectState(tube, true);
    }

    public void Task3()
    {
        AdvanceTask();
        Animate(tube);
        tube.GetComponent<BoxCollider>().enabled = false;
        SetObjectState(thermometer, true);
    }

    public void Task4()
    {
        AdvanceTask();
        Animate(thermometer);
        thermometer.GetComponent<BoxCollider>().enabled = false;
        SetObjectState(matchBox, true);
    }

    public void Task5()
    {
        AdvanceTask();
        Animate(matchBox);
        Invoke(nameof(LightFire), 1.5f);
        Invoke(nameof(TaskComplete), 3f);
    }

    private void AdvanceTask()
    {
        if (currentTask < colliders.Length)
            colliders[currentTask].SetActive(false);

        currentTask++;

        if (currentTask < colliders.Length)
            Invoke(nameof(ActivateCollider), 2f);

        if (currentTask < taskString.Length)
            taskText.text = taskString[currentTask];

        UpdateTaskListUI();
    }

    private void SetObjectState(GameObject obj, bool isActive)
    {
        if (obj != null)
        {
            var drag = obj.GetComponent<DragAndDropModified>();
            if (drag != null)
                drag.enabled = isActive;
        }
    }

    private void Animate(GameObject obj)
    {
        if (obj != null)
        {
            var animator = obj.GetComponent<Animator>();
            if (animator != null)
                animator.enabled = true;

            var drag = obj.GetComponent<DragAndDropModified>();
            if (drag != null)
                drag.enabled = false;
        }
    }

    public void LightFire()
    {
        if (fire != null)
            fire.SetActive(true);
    }

    public void ActivateCollider()
    {
        if (currentTask < colliders.Length)
            colliders[currentTask].SetActive(true);
    }

    private void UpdateTaskListUI()
    {
        string result = "";

        for (int i = 0; i < taskString.Length; i++)
        {
            if (i < currentTask)
            {
                result += $"<color=#AAAAAA><s>• {taskString[i]}</s></color>\n";
            }
            else if (i == currentTask)
            {
                result += $"<b>• {taskString[i]}</b>\n";
            }
            else
            {
                result += $"• {taskString[i]}\n";
            }
        }

        if (taskListText != null)
            taskListText.text = result;
    }
}
