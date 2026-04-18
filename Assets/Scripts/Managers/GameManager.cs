using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public int level = 1;
    public int life = 1;

void Awake()
{
    if (instance != null)
    {
        Destroy(gameObject);
        return;
    }

    instance = this;
    Debug.Log("Life = " + life);
    DontDestroyOnLoad(gameObject);
}
    public void Win()
    {
        Debug.Log("YOU WIN!");
        Invoke("NextLevel", 1f);
    }
   
    public void Lose()
    {
        Debug.Log("YOU LOSE!");
        life++;
        Invoke("Restart", 1f);
    }

    void Restart()
    {

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

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
