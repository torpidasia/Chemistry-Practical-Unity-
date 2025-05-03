using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class DecompositionReaction : MonoBehaviour
{
    public static DecompositionReaction instance;

    [Header("UI")]
    public GameObject taskCompletePanel;
    public TextMeshProUGUI taskText;
    public TextMeshProUGUI taskListText;
    public GameObject taskPanel;
    [Header("Tasks")]
    public string[] taskString;
    public int currentTask;

    [Header("Scene Objects")]
    public GameObject testTube, capillaryTube, matchBox, fire;
    public GameObject[] colliders;

    void Start()
    {
        Screen.orientation = ScreenOrientation.LandscapeLeft;
        instance = this;
        currentTask = 0;

        SetObjectState(testTube, true);
        SetObjectState(capillaryTube, false);
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

    private void OnTriggerEnter(Collider other)
    {
        if (currentTask == 0 && other.gameObject == testTube)
        {
            CompleteTask(testTube, capillaryTube);
        }
        else if (currentTask == 1 && other.gameObject == capillaryTube)
        {
            CompleteTask(capillaryTube, matchBox);
        }
        else if (currentTask == 2 && other.gameObject == matchBox)
        {
            CompleteTask(matchBox, null);
            Invoke(nameof(TaskComplete), 10f);
            Invoke(nameof(LightFire), 1.5f);
        }
    }

    public void OnObjectDropped(GameObject droppedObject)
    {
        if (currentTask == 0 && droppedObject == testTube)
        {
            CompleteTask(testTube, capillaryTube);
        }
        else if (currentTask == 1 && droppedObject == capillaryTube)
        {
            CompleteTask(capillaryTube, matchBox);
        }
        else if (currentTask == 2 && droppedObject == matchBox)
        {
            CompleteTask(matchBox, null);
            Invoke(nameof(TaskComplete), 10f);
            Invoke(nameof(LightFire), 1.5f);
        }
    }

    private void CompleteTask(GameObject completedObject, GameObject nextObject)
    {
        colliders[currentTask].SetActive(false);
        currentTask++;

        if (currentTask < colliders.Length)
            colliders[currentTask].SetActive(true);

        Animate(completedObject);
        if (nextObject != null)
            SetObjectState(nextObject, true);

        taskText.text = currentTask < taskString.Length ? taskString[currentTask] : "Task Completed!";
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
        if (obj == null) return;

        var anim = obj.GetComponent<Animator>();
        if (anim != null) anim.enabled = true;

        var drag = obj.GetComponent<DragAndDropModified>();
        if (drag != null) drag.enabled = false;
    }

    public void LightFire()
    {
        if (fire != null)
            fire.SetActive(true);
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
