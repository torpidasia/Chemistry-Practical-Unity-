using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CopperSulphate : MonoBehaviour
{
    public static CopperSulphate instance;

    [Header("UI")]
    public GameObject taskCompletePanel;
    public GameObject taskPanel;
    public TextMeshProUGUI taskText;        // Current task only
    public TextMeshProUGUI taskListText;    // Full bullet-style list

    [Header("Tasks")]
    public string[] taskString;
    public int currentTask;

    [Header("Scene Objects")]
    public GameObject copperSulphate, beaker, stirer, chinaDish, matchBox, fire, mixture;
    public GameObject[] colliders;

    void Start()
    {
        Screen.orientation = ScreenOrientation.LandscapeLeft;
        instance = this;
        currentTask = 0;

        EnableDrag(copperSulphate, true);

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
        Animate(copperSulphate);
        ChangeTag(copperSulphate);
        EnableDrag(stirer, true);
    }

    public void Task2()
    {
        AdvanceTask();
        Animate(stirer);
        ChangeTag(stirer);
        EnableDrag(beaker, true);
    }

    public void Task3()
    {
        AdvanceTask();
        Animate(beaker);
        ChangeTag(beaker);
        mixture.SetActive(true);
        EnableDrag(chinaDish, true);
    }

    public void Task4()
    {
        AdvanceTask();
        Animate(chinaDish);
        EnableDrag(matchBox, true);
    }

    public void Task5()
    {
        colliders[currentTask].SetActive(false);
        currentTask++;

        Animate(matchBox);
        Invoke(nameof(LightFire), 1.5f);
        taskText.text = taskString[currentTask];
        UpdateTaskListUI();

        Invoke(nameof(TaskComplete), 10f);
    }

    private void AdvanceTask()
    {
        colliders[currentTask].SetActive(false);
        currentTask++;

        if (currentTask < colliders.Length)
            Invoke(nameof(ActivateCollider), 2f);

        taskText.text = currentTask < taskString.Length ? taskString[currentTask] : "Task Completed!";
        UpdateTaskListUI();
    }

    private void Animate(GameObject obj)
    {
        if (obj == null) return;

        var anim = obj.GetComponent<Animator>();
        if (anim != null) anim.enabled = true;

        var drag = obj.GetComponent<DragAndDropModified>();
        if (drag != null) drag.enabled = false;
    }

    private void EnableDrag(GameObject obj, bool enable)
    {
        if (obj == null) return;

        var drag = obj.GetComponent<DragAndDropModified>();
        if (drag != null)
        {
            drag.enabled = enable;
            drag.EnableSelfHighlight(enable);
        }
    }

    public void ChangeTag(GameObject obj)
    {
        if (obj != null)
            obj.tag = "Untagged";
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
        else
            Debug.LogWarning("currentTask index out of bounds in ActivateCollider");
    }

    private void UpdateTaskListUI()
    {
        string result = "";

        for (int i = 0; i < taskString.Length; i++)
        {
            if (i < currentTask)
            {
                result += $"<color=#AAAAAA><s>• {taskString[i]}</s> ✅</color>\n";
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
