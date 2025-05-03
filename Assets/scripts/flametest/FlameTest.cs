using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class FlameTest : MonoBehaviour
{
    public static FlameTest instance;

    [Header("UI")]
    public GameObject taskCompletePanel;
    public TextMeshProUGUI taskText;
    public TextMeshProUGUI taskListText;

    [Header("Tasks")]
    public string[] taskString;
    public int currentTask;

    [Header("Scene Objects")]
    public GameObject strontium, cesium, sodium, lithium, matchBox, fire, stick, lamp;
    public GameObject violetFire, redFire, yellowFire;
    public GameObject[] colliders;
    public GameObject[] glowTargets; // 0=strontium,1=cesium,2=sodium,3=lithium,4=lamp

    private int nextGlowIndex = -1;

    void Start()
    {
        Screen.orientation = ScreenOrientation.LandscapeLeft;

        instance = this;
        currentTask = 0;

        EnableDrag(matchBox, true);
        EnableHighlight(matchBox, true);

        ActivateCollider();
        taskText.text = taskString[currentTask];
        UpdateTaskListUI();

        DisableAllAnimations();
        DisableAllGlowHighlights();
    }

    public void TaskComplete()
    {
        taskCompletePanel.SetActive(true);
        Invoke(nameof(LoadMainMenu), 3f);
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void ActivateNextTask()
    {
        if (currentTask < colliders.Length)
            colliders[currentTask].SetActive(false);

        currentTask++;

        if (currentTask < colliders.Length)
            Invoke(nameof(ActivateCollider), 2f);

        taskText.text = currentTask < taskString.Length ? taskString[currentTask] : "Task Completed";
        UpdateTaskListUI();

        DisableAllGlowHighlights();

        switch (currentTask)
        {
            case 1:
                PlayMatchboxAnimation();
                EnableDrag(matchBox, false);
                EnableDrag(stick, true);
                EnableHighlight(stick, true);
                break;

            case 2:
                PlayStickAnimation("strontium");
                UpdateStickTarget(strontium, 0);
                break;

            case 3:
                PlayStickAnimation("lamp");
                redFire.SetActive(true);
                fire.SetActive(false);
                UpdateStickTarget(cesium, 1);
                break;

            case 4:
                PlayStickAnimation("cesium");
                UpdateStickTarget(fire, -1);
                break;

            case 5:
                PlayStickAnimation("lamp");
                violetFire.SetActive(true);
                redFire.SetActive(false);
                UpdateStickTarget(sodium, 2);
                break;

            case 6:
                PlayStickAnimation("sodium");
                yellowFire.SetActive(true);
                violetFire.SetActive(false);
                UpdateStickTarget(fire, -1);
                break;

            case 7:
                PlayStickAnimation("lamp");
                UpdateStickTarget(lithium, 3);
                break;

            case 8:
                PlayStickAnimation("lithium");
                UpdateStickTarget(fire, -1);
                break;

            case 9:
                PlayStickAnimation("lamp");
                redFire.SetActive(true);
                yellowFire.SetActive(false);
                EnableDrag(stick, false);
                DisableAllGlowHighlights();
                Invoke(nameof(TaskComplete), 3f);
                break;
        }
    }

    void ActivateCollider()
    {
        if (currentTask < colliders.Length)
            colliders[currentTask].SetActive(true);
    }

    void PlayMatchboxAnimation()
    {
        if (matchBox.TryGetComponent(out Animator anim))
            anim.enabled = true;

        Invoke(nameof(LightFire), 1.5f);
    }

    void LightFire()
    {
        fire.SetActive(true);
    }

    void PlayStickAnimation(string triggerName)
    {
        if (stick.TryGetComponent(out Animator anim))
        {
            anim.enabled = true;
            anim.ResetTrigger("strontium");
            anim.ResetTrigger("cesium");
            anim.ResetTrigger("sodium");
            anim.ResetTrigger("lithium");
            anim.ResetTrigger("lamp");
            anim.SetTrigger(triggerName);
        }

        var stickScript = stick.GetComponent<Stick>();
        if (stickScript != null)
        {
            stickScript.OnAnimationStart();
            Invoke(nameof(OnStickAnimationComplete), 1f);
        }

        EnableDrag(stick, false);
        EnableHighlight(stick, true);
    }

    void OnStickAnimationComplete()
    {
        var stickScript = stick.GetComponent<Stick>();
        if (stickScript != null) stickScript.OnAnimationComplete();

        EnableDrag(stick, true);
        EnableHighlight(stick, false);

        if (nextGlowIndex >= 0 && nextGlowIndex < glowTargets.Length)
        {
            EnableHighlight(glowTargets[nextGlowIndex], true);
        }
    }

    void UpdateStickTarget(GameObject newTarget, int glowIndex)
    {
        var drag = stick.GetComponent<DragAndDropModified>();
        if (drag != null)
            drag.targetObject = newTarget;

        nextGlowIndex = glowIndex;
    }

    void EnableDrag(GameObject obj, bool enable)
    {
        var drag = obj.GetComponent<DragAndDropModified>();
        if (drag != null)
            drag.enabled = enable;
    }

    void EnableHighlight(GameObject obj, bool enable)
    {
        var drag = obj.GetComponent<DragAndDropModified>();
        if (drag != null)
        {
            drag.EnableSelfHighlight(enable);
            drag.enabled = true;
        }
    }

    void DisableAllGlowHighlights()
    {
        foreach (var glow in glowTargets)
        {
            if (glow != null)
            {
                var drag = glow.GetComponent<DragAndDropModified>();
                if (drag != null)
                    drag.EnableSelfHighlight(false);
            }
        }
    }

    void DisableAllAnimations()
    {
        if (matchBox.TryGetComponent(out Animator anim1))
            anim1.enabled = false;
        if (stick.TryGetComponent(out Animator anim2))
            anim2.enabled = false;
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
