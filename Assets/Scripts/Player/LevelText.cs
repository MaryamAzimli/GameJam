using TMPro;
using UnityEngine;

public class LevelText : MonoBehaviour
{
    public TextMeshProUGUI textUI;

    void Start()
    {
        ShowText();
    }
    public void ShowText()
    {
            if (GameManager.instance == null)
        return;

        int life = GameManager.instance.life;
        int level = GameManager.instance.level;

        if (level == 1 && life == 1)
        {
            textUI.text = "";
        }
        else if (level == 1 && life == 2)
        {
            textUI.text = "No logical human being would grab honey with bare hands...";
        }
        else if (level == 2)
        {
            textUI.text = "Something feels different in this memory...";
        }
        else if (life >= 10)
        {
            textUI.text = "How many times will you repeat the same mistake?";
        }
    }
}