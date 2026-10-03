using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
    
    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void StopMusic()
    {
        audioSource.Stop();
    }

    public void SetMute(bool mute)
    {
        if (audioSource != null)
            audioSource.mute = mute;
    }

}
