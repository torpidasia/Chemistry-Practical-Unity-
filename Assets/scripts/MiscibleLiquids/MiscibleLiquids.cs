using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class MiscibleLiquids : MonoBehaviour
{
    public static MiscibleLiquids instance;

    [Header("UI")]
    public GameObject taskCompletePanel;
    public TextMeshProUGUI taskText;        // Single-line current task
    public TextMeshProUGUI taskListText;    // Full checklist style

    [Header("Tasks")]
    public string[] taskString;
    public int currentTask;
    public GameObject taskPanel;
    [Header("Scene Objects")]
    public GameObject water, alcohol, oil, WA, OA, WO, stirer;
    public GameObject WA_Mixture, OA_Mixture, WO_Mixture;
    public GameObject water1, water2, alcohol1, alcohol2, oil1, oil2;
    public GameObject[] colliders;

    void Start()
    {
        Screen.orientation = ScreenOrientation.LandscapeLeft;
        instance = this;
        currentTask = 0;

        EnableDragAndHighlight(water, true);
        ActivateCollider();
        taskText.text = taskString[currentTask];
        UpdateTaskListUI();
        DisableAllAnimations();
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
        if (currentTask < colliders.Length)
            colliders[currentTask].SetActive(false);

        currentTask++;

        if (currentTask < colliders.Length)
            Invoke(nameof(ActivateCollider), 2f);

        taskText.text = currentTask < taskString.Length ? taskString[currentTask] : "Task Completed!";
        UpdateTaskListUI();

        switch (currentTask)
        {
            case 1:
                PlayAnimation(water);
                EnableDragAndHighlight(water, false);
                water1.SetActive(true);
                EnableDragAndHighlight(alcohol, true);
                break;

            case 2:
                PlayAnimation(alcohol);
                EnableDragAndHighlight(alcohol, false);
                alcohol1.SetActive(true);
                EnableDragAndHighlight(stirer, true);
                break;

            case 3:
                PlayAnimation(stirer);
                EnableDragAndHighlight(stirer, false);
                water1.SetActive(false);
                alcohol1.SetActive(false);
                WA_Mixture.SetActive(true);
                EnableDragAndHighlight(water, true);
                break;

            case 4:
                PlayAnimation(water);
                EnableDragAndHighlight(water, false);
                water2.SetActive(true);
                EnableDragAndHighlight(oil, true);
                break;

            case 5:
                PlayAnimation(oil);
                EnableDragAndHighlight(oil, false);
                oil1.SetActive(true);
                EnableDragAndHighlight(stirer, true);
                break;

            case 6:
                PlayAnimation(stirer);
                EnableDragAndHighlight(stirer, false);
                water2.SetActive(false);
                oil1.SetActive(false);
                WO_Mixture.SetActive(true);
                EnableDragAndHighlight(oil, true);
                break;

            case 7:
                PlayAnimation(oil);
                EnableDragAndHighlight(oil, false);
                oil2.SetActive(true);
                EnableDragAndHighlight(alcohol, true);
                break;

            case 8:
                PlayAnimation(alcohol);
                EnableDragAndHighlight(alcohol, false);
                alcohol2.SetActive(true);
                EnableDragAndHighlight(stirer, true);

                Invoke(nameof(FinalStirAndComplete), 2f);
                break;
        }
    }

    void FinalStirAndComplete()
    {
        PlayAnimation(stirer);
        EnableDragAndHighlight(stirer, false);
        oil2.SetActive(false);
        alcohol2.SetActive(false);
        OA_Mixture.SetActive(true);

        Invoke(nameof(TaskComplete), 5f);
    }

    void ActivateCollider()
    {
        if (currentTask < colliders.Length)
            colliders[currentTask].SetActive(true);
    }

    void PlayAnimation(GameObject obj)
    {
        var anim = obj.GetComponent<Animator>();
        if (anim != null)
            anim.enabled = true;
    }

    void EnableDragAndHighlight(GameObject obj, bool enable)
    {
        var drag = obj.GetComponent<DragAndDropModified>();
        if (drag != null)
        {
            drag.enabled = enable;
            drag.EnableSelfHighlight(enable);
        }
    }

    void DisableAllAnimations()
    {
        water.GetComponent<Animator>().enabled = false;
        alcohol.GetComponent<Animator>().enabled = false;
        oil.GetComponent<Animator>().enabled = false;
        stirer.GetComponent<Animator>().enabled = false;
    }

    void UpdateTaskListUI()
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
