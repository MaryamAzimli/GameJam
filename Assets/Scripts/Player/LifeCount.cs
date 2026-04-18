using TMPro;
using UnityEngine;

public class LifeUI : MonoBehaviour
{
    public TextMeshProUGUI lifeText;

    void Update()
    {
        if (GameManager.instance == null) return;
        int temp=4-GameManager.instance.life;
        lifeText.text = ""+temp;
    }
}