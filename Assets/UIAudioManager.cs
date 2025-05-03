using UnityEngine;

public class UIAudioManager : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip clickSound;
    public AudioClip backSound;
    public AudioClip hoverSound;

    public void PlayClickSound()
    {
        PlaySound(clickSound);
    }

    public void PlayBackSound()
    {
        PlaySound(backSound);
    }

    public void PlayHoverSound()
    {
        PlaySound(hoverSound);
    }

    private void PlaySound(AudioClip clip)
    {
        if (audioSource != null && clip != null)
        {
            audioSource.PlayOneShot(clip);
        }
    }
}
