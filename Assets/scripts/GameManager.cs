using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    [Header("UI Panels")]
    public GameObject loadingPanel;
    public CanvasGroup loadingCanvasGroup;
    public Image loadingBar;

    public GameObject mainMenuPanel;
  
    public GameObject levelSelectionPanel;
    public GameObject[] levelSelectionInfo;
    public Button backButton;

    [Header("Loading Settings")]
    public float fadeDuration = 1f;

    [Header("Audio")]
    public AudioSource uiAudioSource;
    public AudioClip clickSound;

    private Stack<GameObject> panelHistory = new Stack<GameObject>();

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
        if (backButton != null)
            backButton.onClick.AddListener(OnBackButtonClick);
    }

    void Start()
    {
        Screen.orientation = ScreenOrientation.Portrait;
        StartCoroutine(InitialLoadingSequence());
    }

    private IEnumerator InitialLoadingSequence()
    {
        yield return StartCoroutine(FadeInLoadingScreen());

        float elapsedTime = 0f;
        float dummyDuration = 3f;
        while (elapsedTime < dummyDuration)
        {
            elapsedTime += Time.deltaTime;
            float progress = Mathf.Clamp01(elapsedTime / dummyDuration);
            UpdateLoadingUI(progress);
            yield return null;
        }

        yield return StartCoroutine(FadeOutLoadingScreen());
        mainMenuPanel.SetActive(true);
    }

    private void UpdateLoadingUI(float progress)
    {
        if (loadingBar != null)
            loadingBar.fillAmount = progress;
    }

    private IEnumerator FadeInLoadingScreen()
    {
        loadingPanel.SetActive(true);
        loadingCanvasGroup.alpha = 0f;

        float elapsed = 0f;
        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            loadingCanvasGroup.alpha = Mathf.Clamp01(elapsed / fadeDuration);
            yield return null;
        }
    }

    private IEnumerator FadeOutLoadingScreen()
    {
        float elapsed = 0f;
        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            loadingCanvasGroup.alpha = 1f - Mathf.Clamp01(elapsed / fadeDuration);
            yield return null;
        }

        loadingPanel.SetActive(false);
    }

    #region Main Menu

    public void PlayBtnClick()
    {
        PlayClickSound();
        OpenPanel(levelSelectionPanel);
        mainMenuPanel.SetActive(false);
    }

   
    #endregion


    #region Level Selection

    public void LevelSelectionInfo(int index)
    {
        PlayClickSound();
        if (index >= 0 && index < levelSelectionInfo.Length)
        {
            OpenPanel(levelSelectionInfo[index]);
            levelSelectionPanel.SetActive(false);
        }
    }

    public void LevelSelectionBtnClick(int sceneIndex)
    {
        PlayClickSound();
        StartCoroutine(LoadSceneAsync(sceneIndex));
    }

    private IEnumerator LoadSceneAsync(int sceneIndex)
    {
        yield return StartCoroutine(FadeInLoadingScreen());

        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex);
        operation.allowSceneActivation = false;

        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / 0.9f);
            UpdateLoadingUI(progress);

            if (operation.progress >= 0.9f)
            {
                UpdateLoadingUI(1f);
                yield return new WaitForSeconds(0.5f);
                operation.allowSceneActivation = true;
            }

            yield return null;
        }
    }

    #endregion

    #region Panel Navigation

    private void OpenPanel(GameObject panel)
    {
        if (panel != null && !panel.activeSelf)
        {
            if (mainMenuPanel.activeSelf) panelHistory.Push(mainMenuPanel);
         
            else if (levelSelectionPanel.activeSelf) panelHistory.Push(levelSelectionPanel);
            else
            {
                foreach (var infoPanel in levelSelectionInfo)
                {
                    if (infoPanel.activeSelf)
                    {
                        panelHistory.Push(infoPanel);
                        break;
                    }
                }
            }

            panel.SetActive(true);
        }
    }

    public void OnBackButtonClick()
    {
        PlayClickSound();
        BackToPreviousPanel();
    }

    private void BackToPreviousPanel()
    {
        if (panelHistory.Count > 0)
        {
            GameObject previousPanel = panelHistory.Pop();

            mainMenuPanel.SetActive(false);
          
            levelSelectionPanel.SetActive(false);
            foreach (var infoPanel in levelSelectionInfo)
                infoPanel.SetActive(false);

            previousPanel.SetActive(true);
        }
    }

    #endregion

    #region Audio

    public void PlayClickSound()
    {
        if (uiAudioSource != null && clickSound != null)
        {
            uiAudioSource.PlayOneShot(clickSound);
        }
    }

    #endregion
}
