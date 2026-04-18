using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [Header("Panels")]
    public GameObject mainPanel;
    public GameObject settingsPanel;
    public GameObject backStoryPanel;

    void Start()
    {
        mainPanel.SetActive(true);
        settingsPanel.SetActive(false);
        backStoryPanel.SetActive(false);
    }

    // Main buttons
    public void PlayGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void OpenSettings()
    {
        mainPanel.SetActive(false);
        settingsPanel.SetActive(true);
    }

    public void OpenBackStory()
    {
        mainPanel.SetActive(false);
        backStoryPanel.SetActive(true);
    }

    public void QuitGame()
    {
        Debug.Log("Quit");
        Application.Quit();
    }

    // Back button (used in Settings and BackStory panels)
    public void BackToMain()
    {
        settingsPanel.SetActive(false);
        backStoryPanel.SetActive(false);
        mainPanel.SetActive(true);
    }
}