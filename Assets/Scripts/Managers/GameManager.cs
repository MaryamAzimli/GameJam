using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public int level = 1;
    public int life = 1;

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
        Debug.Log("YOU WIN!");

        if (winPanelUI != null)
        {
            Debug.Log("Calling winPanelUI.Show()");
            winPanelUI.Show();
        }
        else
        {
            Debug.Log("winPanelUI is NULL");
        }
    }

    public void Lose()
    {
        Debug.Log("YOU LOSE!");
        life++;

        if (losePanelUI != null)
        {
            Debug.Log("Calling losePanelUI.Show()");
            losePanelUI.Show();
        }
        else
        {
            Debug.Log("losePanelUI is NULL");
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
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    void NextLevel()
{
    level++;

    int nextIndex = SceneManager.GetActiveScene().buildIndex + 1;

    if (nextIndex < SceneManager.sceneCountInBuildSettings)
    {
        SceneManager.LoadScene(nextIndex);
    }
    else
    {
        Debug.Log("No more levels!");
        // Later we can load a win screen here
    }
}
    }
}
