using UnityEngine;
using UnityEngine.SceneManagement;

public GameObject losePanel;

public class GameManager : MonoBehaviour
{
    public void Win()
    {
        Debug.Log("YOU WIN!");
        Invoke("NextLevel", 1f);
    }

    public void Lose()
    {
        Debug.Log("YOU LOSE!");
        losePanel.SetActive(true);
        Invoke("Restart", 1f);
    }

    void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    void NextLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}