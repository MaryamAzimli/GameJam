using UnityEngine;

public class WinPanelUI : MonoBehaviour
{
    public void Show()
    {
        Debug.Log("WinPanelUI Show called on: " + gameObject.name);
        gameObject.SetActive(true);
        Debug.Log("Win panel activeSelf after Show: " + gameObject.activeSelf);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}