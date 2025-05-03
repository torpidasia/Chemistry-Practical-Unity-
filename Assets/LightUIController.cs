using UnityEngine;
using UnityEngine.UI;
using TMPro; // <- IMPORTANT: using TextMeshPro

public class SceneUIFlow : MonoBehaviour
{
    [Header("Panels")]
    public GameObject tutorialPanel;
    public GameObject lightPanel;
    public GameObject apparatusPanel;
    public GameObject startPanel;
    [Header("Buttons")]
    public Button tutorialButton;
    public Button lightButton;
    public Button apparatusButton;

    [Header("Text")]
    public TextMeshProUGUI instructionText; // <-- TextMeshPro version

    [Header("Scene Objects")]
    public Light sceneLight;
    public GameObject[] apparatusObjects;

    [Header("Audio")]
    public AudioClip buttonClickSound;
    private AudioSource audioSource;

    private void Awake()
    {
        Screen.orientation = ScreenOrientation.LandscapeLeft; // or LandscapeRight

        audioSource = gameObject.AddComponent<AudioSource>();

        tutorialPanel.SetActive(true);
        lightPanel.SetActive(false);
        apparatusPanel.SetActive(false);
        sceneLight.enabled = false;

        foreach (var obj in apparatusObjects)
            obj.SetActive(false);

        tutorialButton.onClick.AddListener(() => {
            PlaySound();
            ShowLightPanel();
        });

        lightButton.onClick.AddListener(() => {
            PlaySound();
            TurnOnLight();
        });

        apparatusButton.onClick.AddListener(() => {
            PlaySound();
            ShowApparatus();
        });
    }

    void PlaySound()
    {
        if (buttonClickSound != null)
            audioSource.PlayOneShot(buttonClickSound);
    }

    void ShowLightPanel()
    {
        tutorialPanel.SetActive(false);
        lightPanel.SetActive(true);
       // instructionText.text = "<b>LET'S START<\b>\nTURN ON LIGHT";
    }

    void TurnOnLight()
    {
        if (sceneLight != null)
            sceneLight.enabled = true;

       // instructionText.text = "LET'S HAVE APPARATUS";
        lightButton.gameObject.SetActive(false);
        lightPanel.SetActive(false);
     
        Debug.Log("Showing Apparatus Panel");
        if (apparatusPanel != null)
            apparatusPanel.SetActive(true);
        else
            Debug.LogWarning("Apparatus Panel is NOT assigned!");
    }

    void ShowApparatus()
    {
        startPanel.SetActive(false);
        apparatusPanel.SetActive(false);
        foreach (var obj in apparatusObjects)
            obj.SetActive(true);
    }

}
