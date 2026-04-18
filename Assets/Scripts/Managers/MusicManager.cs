using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class MusicManager : MonoBehaviour
{
    public static MusicManager instance;

    private AudioSource audioSource;

    private bool forceStopped = false;

    private void Awake()
    {
        instance = this;
        audioSource = GetComponent<AudioSource>();

        Debug.Log("MusicManager Awake on: " + gameObject.name);
        Debug.Log("MusicManager audioSource = " + audioSource);
    }

    public void StopMusic()
    {
        forceStopped = true;
        audioSource.Stop();
        audioSource.volume = 0f;
    }

    private void Start()
    {
        ApplyMusicState();

        if (SettingsManager.Instance != null)
        {
            SettingsManager.Instance.OnMusicChanged += HandleMusicChanged;
        }
    }

    private void OnDestroy()
    {
        if (SettingsManager.Instance != null)
        {
            SettingsManager.Instance.OnMusicChanged -= HandleMusicChanged;
        }
    }

    private void HandleMusicChanged(bool isMusicOn)
    {
        ApplyMusicState();
    }

    private void ApplyMusicState()
    {
        if (forceStopped)
            return;

        if (SettingsManager.Instance == null)
            return;

        if (SettingsManager.Instance.IsMusicOn)
        {
            if (!audioSource.isPlaying)
                audioSource.Play();

            audioSource.volume = 1f;
        }
        else
        {
            audioSource.volume = 0f;
        }
    }
}