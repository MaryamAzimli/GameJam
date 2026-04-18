using UnityEngine;

public class SFXManager : MonoBehaviour
{
    public static SFXManager instance;

    public AudioClip trapSFX;
    public AudioClip winSFX;

    private AudioSource audioSource;

    void Awake()
    {
        instance = this;
        audioSource = GetComponent<AudioSource>();
    }

    public void PlayTrapSFX()
    {
        if (trapSFX != null)
        {
            audioSource.PlayOneShot(trapSFX);
        }
    }

    public void PlayWinSFX()
    {
        if (winSFX != null)
        {
            audioSource.PlayOneShot(winSFX);
        }
    }
}