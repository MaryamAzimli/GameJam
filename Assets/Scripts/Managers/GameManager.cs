using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public int level = 1;
    public int life = 1;
    public bool isGameOver = false;

    public LosePanelUI losePanelUI;
    public WinPanelUI winPanelUI;

    void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        isGameOver = false;
        Debug.Log("Life = " + life);
    }

    void Start()
    {
        if (losePanelUI == null)
        {
            GameObject loseObj = GameObject.Find("LoseWindowPanel");
            if (loseObj != null)
                losePanelUI = loseObj.GetComponent<LosePanelUI>();
        }

        if (winPanelUI == null)
        {
            GameObject winObj = GameObject.Find("WinWindowPanel");
            if (winObj != null)
                winPanelUI = winObj.GetComponent<WinPanelUI>();
        }

        Debug.Log("losePanelUI after Start = " + losePanelUI);
        Debug.Log("winPanelUI after Start = " + winPanelUI);
    }

    public void Win()
    {
        if (isGameOver) return;
        isGameOver = true;

        Debug.Log("YOU WIN!");

        if (winPanelUI != null)
        {
            Debug.Log("Calling winPanelUI.Show()");
            winPanelUI.Show();
        }
    }

    public void Lose()
    {
        if (isGameOver) return;
        isGameOver = true;

        Debug.Log("YOU LOSE!");
        life++;

        if (losePanelUI != null)
        {
            Debug.Log("Calling losePanelUI.Show()");
            losePanelUI.Show();
        }
    }

    public void Restart()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void NextLevel()
    {
        level++;

        int nextIndex = SceneManager.GetActiveScene().buildIndex + 1;

        if (nextIndex < SceneManager.sceneCountInBuildSettings)
        {
            Time.timeScale = 1f;
            SceneManager.LoadScene(nextIndex);
        }
        else
        {
            Debug.Log("No more levels!");
        }
    }
}