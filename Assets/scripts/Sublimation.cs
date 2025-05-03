using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Sublimation : MonoBehaviour
{
    public static Sublimation instance;

    [Header("UI")]
    public GameObject taskCompletePanel;
    public TextMeshProUGUI taskText;        // Shows current task only
    public TextMeshProUGUI taskListText;    // Shows all tasks with bullets
    public GameObject taskPanel;
    [Header("Tasks")]
    public string[] taskString;
    public int currentTask;

    [Header("Scene Objects")]
    public GameObject NH4Cl, wireGuaze, chinaDish, funnel, cotton, matchBox, fire, mixture;
    public GameObject[] colliders;

    void Start()
    {
        Screen.orientation = ScreenOrientation.LandscapeLeft;
        instance = this;
        currentTask = 0;

        EnableDragAndHighlight(NH4Cl, true);

        if (colliders.Length > 0)
        {
            colliders[currentTask].SetActive(true);
        }

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

    public void ActivateNextTask()
    {
        if (currentTask >= colliders.Length)
        {
            Debug.Log("No more tasks left.");
            return;
        }

        if (currentTask < colliders.Length)
        {
            colliders[currentTask].SetActive(false);
        }

        currentTask++;

        if (currentTask < colliders.Length)
        {
            Invoke(nameof(ActivateCollider), 2f);
        }

        switch (currentTask)
        {
            case 1:
                PlayAnimation(NH4Cl);
                mixture.SetActive(true);
                EnableDragAndHighlight(NH4Cl, false);
                EnableDragAndHighlight(wireGuaze, true);
                break;

            case 2:
                PlayAnimation(wireGuaze);
                EnableDragAndHighlight(wireGuaze, false);
                EnableDragAndHighlight(chinaDish, true);
                break;

            case 3:
                PlayAnimation(chinaDish);
                EnableDragAndHighlight(chinaDish, false);
                EnableDragAndHighlight(funnel, true);
                break;

            case 4:
                PlayAnimation(funnel);
                EnableDragAndHighlight(funnel, false);
                EnableDragAndHighlight(cotton, true);
                break;

            case 5:
                PlayAnimation(cotton);
                EnableDragAndHighlight(cotton, false);
                EnableDragAndHighlight(matchBox, true);
                break;

            case 6:
                PlayAnimation(matchBox);
                EnableDragAndHighlight(matchBox, false);
                Invoke(nameof(LightFire), 1.5f);
               
                Invoke(nameof(TaskComplete), 10f);
                break;
        }

        // Update task text
        taskText.text = currentTask < taskString.Length ? taskString[currentTask] : "Task Completed!";
        UpdateTaskListUI();
    }

    public void LightFire()
    {
        fire.SetActive(true);
    }

    public void ActivateCollider()
    {
        if (currentTask < colliders.Length)
        {
            colliders[currentTask].SetActive(true);
        }
    }

    private void EnableDragAndHighlight(GameObject obj, bool enable)
    {
        var dragComponent = obj.GetComponent<DragAndDropModified>();
        if (dragComponent != null)
        {
            dragComponent.enabled = enable;
            dragComponent.EnableSelfHighlight(enable);
        }
    }

    private void PlayAnimation(GameObject obj)
    {
        var animator = obj.GetComponent<Animator>();
        if (animator != null)
        {
            animator.enabled = true;
        }
    }

    private void DisableAllAnimations()
    {
        NH4Cl.GetComponent<Animator>().enabled = false;
        wireGuaze.GetComponent<Animator>().enabled = false;
        chinaDish.GetComponent<Animator>().enabled = false;
        funnel.GetComponent<Animator>().enabled = false;
        cotton.GetComponent<Animator>().enabled = false;
        matchBox.GetComponent<Animator>().enabled = false;
    }

    public void AnimateMatchbox()
    {
        Debug.Log("Animating Matchbox!");
        PlayAnimation(matchBox);
        EnableDragAndHighlight(matchBox, false);
        Invoke(nameof(LightFire), 1.5f);
        Invoke(nameof(TaskComplete), 10f);
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
