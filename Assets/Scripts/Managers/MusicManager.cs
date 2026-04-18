using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class MusicManager : MonoBehaviour
{
    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
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