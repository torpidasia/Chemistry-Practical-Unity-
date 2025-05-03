using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Loading : MonoBehaviour
{
    public Image loadingBar;
    public TextMeshProUGUI loadingText;
    public float loadDuration = 5f;
    public GameManager gm;
    public CanvasGroup loadingScreenCanvasGroup;
    public float fadeDuration = 1f;

    private void Start()
    {
        StartCoroutine(FadeInLoadingScreen());
    }

    private IEnumerator LoadProgress()
    {
        float elapsedTime = 0f;

        while (elapsedTime < loadDuration)
        {
            elapsedTime += Time.deltaTime;
            float progress = Mathf.Clamp01(elapsedTime / loadDuration);
            UpdateLoadingUI(progress);
            yield return null;
        }

        UpdateLoadingUI(1f);
        StartCoroutine(FadeOutLoadingScreen());
    }

    private void UpdateLoadingUI(float progress)
    {
        if (loadingBar != null)
        {
            loadingBar.fillAmount = progress;
        }

        if (loadingText != null)
        {
            loadingText.text = "Loading... " + (progress * 100).ToString("F0") + "%";
        }
    }

    private IEnumerator FadeInLoadingScreen()
    {
        float elapsedTime = 0f;
        loadingScreenCanvasGroup.alpha = 0;
        loadingScreenCanvasGroup.gameObject.SetActive(true);

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            loadingScreenCanvasGroup.alpha = Mathf.Clamp01(elapsedTime / fadeDuration);
            yield return null;
        }

        StartCoroutine(LoadProgress());
    }

    private IEnumerator FadeOutLoadingScreen()
    {
        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            loadingScreenCanvasGroup.alpha = 1 - Mathf.Clamp01(elapsedTime / fadeDuration);
            yield return null;
        }

        OnLoadingComplete();
    }

    private void OnLoadingComplete()
    {
        Debug.Log("Loading Complete!");
        gm.mainMenuPanel.SetActive(true);
        loadingScreenCanvasGroup.gameObject.SetActive(false);
    }
}
