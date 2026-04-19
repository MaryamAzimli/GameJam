using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public int level = 1;
    public int life = 1;
    public bool isGameOver = false;

    [Header("Puzzle State")]
    public int currentStep = 1;
    public bool hasFood = false;
    public int activeFoodID = 0; // Hangi yeme?i ald???m?z? tutar

    [Header("Audio Setup")]
    public AudioSource bgmSource;

    public LosePanelUI losePanelUI;
    public WinPanelUI winPanelUI;

    void Awake()
    {
        if (instance != null && instance != this) { Destroy(gameObject); return; }
        instance = this;
    }

    public void Win()
    {
        if (isGameOver) return;
        isGameOver = true;
        if (bgmSource != null) bgmSource.Stop();
        if (winPanelUI != null) winPanelUI.Show();
    }

    public void Lose()
    {
        if (isGameOver) return;
        life++;
        if (life >= 4)
        {
            isGameOver = true;
            if (bgmSource != null) bgmSource.Stop();
            if (losePanelUI != null) losePanelUI.Show();
        }
    }

    public void Restart()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}