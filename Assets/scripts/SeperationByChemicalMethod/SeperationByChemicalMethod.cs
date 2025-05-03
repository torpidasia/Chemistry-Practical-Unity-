using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SeperationByChemicalMethod : MonoBehaviour
{
    public static SeperationByChemicalMethod instance;

    [Header("UI")]
    public GameObject taskCompletePanel;
    public GameObject taskPanel;
    public TextMeshProUGUI taskText;
    public TextMeshProUGUI taskListText;

    [Header("Tasks")]
    public string[] taskString;
    public int currentTask;

    [Header("Scene Objects")]
    public GameObject beaker, stirer, wireGuaze, chinaDish, matchBox, fire;
    public GameObject salt, water, saltWater;
    public GameObject[] colliders;

    void Start()
    {
        Screen.orientation = ScreenOrientation.LandscapeLeft;
        instance = this;
        currentTask = 0;

        EnableDragAndHighlight(beaker, true);

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
                Animate(beaker);
                EnableDragAndHighlight(beaker, false);
                EnableDragAndHighlight(stirer, true);
                water.SetActive(true);
                break;

            case 2:
                Animate(stirer);
                EnableDragAndHighlight(stirer, false);
                EnableDragAndHighlight(wireGuaze, true);

                salt.SetActive(false);
                water.SetActive(false);
                saltWater.SetActive(true);
                break;

            case 3:
                Animate(wireGuaze);
                EnableDragAndHighlight(wireGuaze, false);
                EnableDragAndHighlight(chinaDish, true);
                break;

            case 4:
                Animate(chinaDish);
                EnableDragAndHighlight(chinaDish, false);
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
        fire.SetActive(true);
    }

    public void ActivateCollider()
    {
        if (currentTask < colliders.Length)
            colliders[currentTask].SetActive(true);
    }

    private void EnableDragAndHighlight(GameObject obj, bool enable)
    {
        var drag = obj.GetComponent<DragAndDropModified>();
        if (drag != null)
        {
            drag.enabled = enable;
            drag.EnableSelfHighlight(enable);
        }
    }

    private void Animate(GameObject obj)
    {
        var anim = obj.GetComponent<Animator>();
        if (anim != null)
            anim.enabled = true;

        var drag = obj.GetComponent<DragAndDropModified>();
        if (drag != null)
            drag.enabled = false;
    }

    private void DisableAllAnimations()
    {
        beaker.GetComponent<Animator>().enabled = false;
        stirer.GetComponent<Animator>().enabled = false;
        wireGuaze.GetComponent<Animator>().enabled = false;
        chinaDish.GetComponent<Animator>().enabled = false;
        matchBox.GetComponent<Animator>().enabled = false;
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

    public void AnimateMatchbox()
    {
        Animate(matchBox);
        EnableDragAndHighlight(matchBox, false);
        Invoke(nameof(LightFire), 1.5f);
        Invoke(nameof(TaskComplete), 10f);
    }
}
