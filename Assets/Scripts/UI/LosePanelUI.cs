using UnityEngine;
using System.Collections;

public class LosePanelUI : MonoBehaviour
{
    public float moveDuration = 1.0f;
    public float hiddenY = -1000f;
    public float shownY = 0f;

    public AudioClip riseLoseSFX;

    private RectTransform rectTransform;
    private AudioSource audioSource;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        audioSource = GetComponent<AudioSource>();
    }

    public void Show()
    {
        if (rectTransform == null)
            rectTransform = GetComponent<RectTransform>();

        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();

        if (rectTransform == null)
        {
            Debug.LogError("LosePanelUI: RectTransform still NULL on " + gameObject.name);
            return;
        }

        StopAllCoroutines();

        rectTransform.anchoredPosition = new Vector2(0f, hiddenY);
        gameObject.SetActive(true);

        if (MusicManager.instance != null)
        {
            MusicManager.instance.StopMusic();
        }

        if (audioSource != null && riseLoseSFX != null)
        {
            audioSource.clip = riseLoseSFX;
            audioSource.Play();
        }

        StartCoroutine(SlideUp());
    }

    private IEnumerator SlideUp()
    {
        float elapsed = 0f;
        Vector2 startPos = new Vector2(0f, hiddenY);
        Vector2 endPos = new Vector2(0f, shownY);

        while (elapsed < moveDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / moveDuration;
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