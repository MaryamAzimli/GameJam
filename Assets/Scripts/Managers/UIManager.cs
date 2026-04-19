using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public GameObject pauseMenuPanel;
    public GameObject pauseWindowPanel;
    public GameObject settingsPanel;

    [Header("Riddle Setup")]
    public GameObject riddlePanel; // Buraya Hierarchy'deki RiddlePanel'i sürükle

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

    // --- RIDDLE (BILMECE) FONKSIYONLARI ---

    public void OpenRiddle()
    {
        if (riddlePanel != null)
        {
            riddlePanel.SetActive(true);
            Time.timeScale = 0f; // Okurken oyun dursun
        }
    }

    public void CloseRiddle()
    {
        if (riddlePanel != null)
        {
            riddlePanel.SetActive(false);
            Time.timeScale = 1f; // Kapat?nca oyun devam etsin
        }
    }

    // --- MENÜ KONTROLLER? ---

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