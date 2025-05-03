using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class seperateNepthaleneFromSand : MonoBehaviour
{
    public static seperateNepthaleneFromSand instance;

    [Header("UI")]
    public GameObject taskCompletePanel;
    public TextMeshProUGUI taskText;        // Shows current task only
    public TextMeshProUGUI taskListText;    // Shows all tasks with bullets
    public GameObject taskPanel;
    [Header("Tasks")]
    public string[] taskString;
    public int currentTask;

    [Header("Scene Objects")]
    public GameObject wireGuaze, chinaDish, funnel, cotton, matchBox, fire;
    public GameObject[] colliders;

    void Start()
    {
        Screen.orientation = ScreenOrientation.LandscapeLeft;
        instance = this;
        currentTask = 0;

        EnableDragAndHighlight(wireGuaze, true);

        if (colliders.Length > 0)
            colliders[currentTask].SetActive(true);

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
        if (currentTask >= colliders.Length - 1)
        {
            Debug.Log("No more tasks left.");
            return;
        }

        if (currentTask < colliders.Length)
            colliders[currentTask].SetActive(false);

        currentTask++;

        if (currentTask < colliders.Length)
            Invoke(nameof(ActivateCollider), 2f);

        switch (currentTask)
        {
            case 1:
                Animate(wireGuaze);
                EnableDragAndHighlight(wireGuaze, false);
                EnableDragAndHighlight(chinaDish, true);
                break;

            case 2:
                Animate(chinaDish);
                EnableDragAndHighlight(chinaDish, false);
                EnableDragAndHighlight(funnel, true);
                break;

            case 3:
                Animate(funnel);
                EnableDragAndHighlight(funnel, false);
                EnableDragAndHighlight(cotton, true);
                break;

            case 4:
                Animate(cotton);
                EnableDragAndHighlight(cotton, false);
                EnableDragAndHighlight(matchBox, true);
                break;

            case 5:
                Animate(matchBox);
                EnableDragAndHighlight(matchBox, false);
                Invoke(nameof(LightFire), 1.5f);
                Invoke(nameof(TaskComplete), 10f);
                break;
        }

        taskText.text = currentTask < taskString.Length ? taskString[currentTask] : "Task Completed!";
        UpdateTaskListUI();
    }

    public void LightFire()
    {
        fire?.SetActive(true);
    }

    public void ActivateCollider()
    {
        if (currentTask < colliders.Length)
            colliders[currentTask].SetActive(true);
    }

    private void EnableDragAndHighlight(GameObject obj, bool enable)
    {
        if (obj == null) return;

        var dragComponent = obj.GetComponent<DragAndDropModified>();
        if (dragComponent != null)
        {
            dragComponent.enabled = enable;
            dragComponent.EnableSelfHighlight(enable);
        }
    }

    private void Animate(GameObject obj)
    {
        if (obj == null) return;

        var animator = obj.GetComponent<Animator>();
        if (animator != null)
            animator.enabled = true;

        var drag = obj.GetComponent<DragAndDropModified>();
        if (drag != null)
            drag.enabled = false;
    }

    private void DisableAllAnimations()
    {
        if (wireGuaze != null) wireGuaze.GetComponent<Animator>().enabled = false;
        if (chinaDish != null) chinaDish.GetComponent<Animator>().enabled = false;
        if (funnel != null) funnel.GetComponent<Animator>().enabled = false;
        if (cotton != null) cotton.GetComponent<Animator>().enabled = false;
        if (matchBox != null) matchBox.GetComponent<Animator>().enabled = false;
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
