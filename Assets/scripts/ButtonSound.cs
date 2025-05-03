using UnityEngine;
using UnityEngine.UI;

public class ButtonSound : MonoBehaviour
{
    public AudioClip buttonClickSound; // The sound to play when the button is clicked
    private AudioSource audioSource;

    void Start()
    {
        audioSource = GameObject.Find("ButtonSoundManager").GetComponent<AudioSource>();

        // Ensure the AudioSource is found
        if (audioSource == null)
        {
            Debug.LogError("AudioSource not found on ButtonSoundManager GameObject.");
            return;
        }

        // Get the Button component and add the listener
        Button button = GetComponent<Button>();
        if (button != null)
        {
            button.onClick.AddListener(PlayButtonClickSound);
        }
        else
        {
            Debug.LogError("Button component not found on this GameObject.");
        }
    }

    void PlayButtonClickSound()
    {
        audioSource.PlayOneShot(buttonClickSound);
    }
}
