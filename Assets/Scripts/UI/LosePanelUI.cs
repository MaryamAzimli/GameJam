using UnityEngine;

public class LosePanelUI : MonoBehaviour
{
    public void Show()
    {
        Debug.Log("LosePanelUI Show called on: " + gameObject.name);
        gameObject.SetActive(true);
        Debug.Log("Lose panel activeSelf after Show: " + gameObject.activeSelf);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}