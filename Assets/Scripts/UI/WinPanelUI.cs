using UnityEngine;
using System.Collections;

public class WinPanelUI : MonoBehaviour
{
    public float moveDuration = 1.0f;
    public float hiddenY = -1000f;
    public float shownY = 0f;

    public AudioClip riseWinSFX;

    private RectTransform rectTransform;
    private AudioSource audioSource;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        audioSource = GetComponent<AudioSource>();
    }

    public void Show()
    {
        Debug.Log("WinPanelUI.Show on " + gameObject.name);
        Debug.Log("riseWinSFX = " + riseWinSFX);

        if (rectTransform == null)
            rectTransform = GetComponent<RectTransform>();

        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();

        if (rectTransform == null)
        {
            Debug.LogError("WinPanelUI: RectTransform still NULL on " + gameObject.name);
            return;
        }

        StopAllCoroutines();

        rectTransform.anchoredPosition = new Vector2(0f, hiddenY);
        gameObject.SetActive(true);

        if (MusicManager.instance != null)
        {
            MusicManager.instance.StopMusic();
        }

        if (audioSource != null && riseWinSFX != null)
        {
            audioSource.clip = riseWinSFX;
            audioSource.Play();
        }

        StartCoroutine(SlideUp());
    }
private IEnumerator SlideUp()
{
    float elapsed = 0f;
    Vector2 startPos = new Vector2(0f, hiddenY);
    Vector2 endPos = new Vector2(0f, shownY);

    // Use unscaledDeltaTime so the animation works even when Time.timeScale = 0
    while (elapsed < moveDuration)
    {
        elapsed += Time.unscaledDeltaTime; 
        float t = elapsed / moveDuration;
        
        // Use a SmoothStep or Lerp to move the panel
        rectTransform.anchoredPosition = Vector2.Lerp(startPos, endPos, t);
        yield return null;
    }

    rectTransform.anchoredPosition = endPos;

    if (audioSource != null && audioSource.isPlaying)
    {
        audioSource.Stop();
    }
}

    public void Hide()
    {
        StopAllCoroutines();

        if (audioSource != null && audioSource.isPlaying)
        {
            audioSource.Stop();
        }

        gameObject.SetActive(false);
    }
}