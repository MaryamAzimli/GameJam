using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class IntroManager : MonoBehaviour
{
    public VideoPlayer videoPlayer;
    public string nextSceneName = "StartMainMenu"; // first scene

    void Start()
    {
        videoPlayer.loopPointReached += OnVideoFinished;
        videoPlayer.Play();
    }

    void Update()
    {
        // Allow skipping with tap/click or any key
        if (Input.anyKeyDown || Input.touchCount > 0)
        {
            LoadNextScene();
        }
    }

    void OnVideoFinished(VideoPlayer vp)
    {
        LoadNextScene();
    }

    void LoadNextScene()
    {
        videoPlayer.Stop();
        SceneManager.LoadScene(nextSceneName);
    }
}