using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public GameObject pauseMenuPanel;
    public GameObject pauseWindowPanel;
    public GameObject settingsPanel;

    public void RestartLevel()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void PauseGame()
    {
        pauseMenuPanel.SetActive(true);
        pauseWindowPanel.SetActive(true);
        settingsPanel.SetActive(false);
        Time.timeScale = 0f;
    }

    public void ResumeGame()
    {
        pauseMenuPanel.SetActive(false);
        Time.timeScale = 1f;
    }

    public void GoToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }

    public void OpenSettings()
    {
        pauseWindowPanel.SetActive(false);
        settingsPanel.SetActive(true);
    }

    public void CloseSettings()
    {
        settingsPanel.SetActive(false);
        pauseWindowPanel.SetActive(true);
    }
}