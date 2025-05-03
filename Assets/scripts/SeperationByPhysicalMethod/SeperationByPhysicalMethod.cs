using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SeperationByPhysicalMethod : MonoBehaviour
{
    public static SeperationByPhysicalMethod instance;

    [Header("UI")]
    public GameObject taskCompletePanel;
    public TextMeshProUGUI taskText;        // Shows current task only
    public TextMeshProUGUI taskListText;    // Shows all tasks with bullets
    public GameObject taskPanel;

    [Header("Tasks")]
    public string[] taskString;
    public int currentTask;

    [Header("Scene Objects")]
    public GameObject iron, sand, chinaDish, magnet, dishSand, dishIron;
    public GameObject[] colliders;

    void Start()
    {
        Screen.orientation = ScreenOrientation.LandscapeLeft;
        instance = this;
        currentTask = 0;

        EnableDragAndHighlight(sand, true);

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
        if (currentTask >= colliders.Length)
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
                Animate(sand);
                EnableDragAndHighlight(sand, false);
                EnableDragAndHighlight(iron, true);
                Invoke(nameof(TriggerSandActivation), 1f);
                break;

            case 2:
                Animate(iron);
                EnableDragAndHighlight(iron, false);
                EnableDragAndHighlight(magnet, true);
                dishIron.SetActive(true);
                break;

            case 3:
                Animate(magnet);
                EnableDragAndHighlight(magnet, false);
                dishIron.SetActive(false);
                Invoke(nameof(TaskComplete), 6f);
                break;
        }

        taskText.text = currentTask < taskString.Length ? taskString[currentTask] : "Task Completed!";
        UpdateTaskListUI();
    }

    public void ActivateCollider()
    {
        if (currentTask < colliders.Length)
            colliders[currentTask].SetActive(true);
    }

    private void EnableDragAndHighlight(GameObject obj, bool enable)
    {
        if (obj != null)
        {
            var dragComponent = obj.GetComponent<DragAndDropModified>();
            if (dragComponent != null)
            {
                dragComponent.enabled = enable;
                dragComponent.EnableSelfHighlight(enable);
            }
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

    private void DisableAllAnimations()
    {
        if (sand != null) sand.GetComponent<Animator>().enabled = false;
        if (iron != null) iron.GetComponent<Animator>().enabled = false;
        if (magnet != null) magnet.GetComponent<Animator>().enabled = false;
    }

    public void TriggerSandActivation()
    {
        dishSand.SetActive(!dishSand.activeInHierarchy);
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
