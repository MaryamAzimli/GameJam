using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("Game State")]
    public int level = 1;
    public int life = 1; // Starts at 1, fails at 4 (3 lives total)
    public bool isGameOver = false;

    [Header("Puzzle State")]
    public int currentStep = 1;
    public bool hasFood = false;
    public int activeFoodID = 0;

    [Header("References")]
    public AudioSource bgmSource;
    public LosePanelUI losePanelUI;
    public WinPanelUI winPanelUI;

    void Awake()
    {
        if (instance != null && instance != this) { Destroy(gameObject); return; }
        instance = this;
        
        // Ensure time is running when the level starts
        Time.timeScale = 1f; 
    }

    public void Win()
    {
        if (isGameOver) return;
        isGameOver = true;

        Debug.Log("Win triggered!");

        if (bgmSource != null) bgmSource.Stop();
        
        // Show the UI and stop the game world
        if (winPanelUI != null) 
        {
            winPanelUI.Show();
            Time.timeScale = 0f; 
        }
    }

    public void Lose()
    {
        if (isGameOver) return;
        
        // This increments life counter. 
        // If life is 1, 2, 3 you keep playing. 4 is Game Over.
        life++; 
        
        if (life >= 4)
        {
            isGameOver = true;
            if (bgmSource != null) bgmSource.Stop();
            
            if (losePanelUI != null) 
            {
                losePanelUI.Show();
                Time.timeScale = 0f;
            }
        }
    }

    public void Restart()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}