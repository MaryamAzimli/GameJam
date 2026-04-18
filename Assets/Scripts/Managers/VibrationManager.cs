using UnityEngine;

public class VibrationManager : MonoBehaviour
{
    public static VibrationManager Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void Vibrate()
    {
        if (SettingsManager.Instance == null)
            return;

        if (!SettingsManager.Instance.IsVibrationOn)
            return;

#if UNITY_ANDROID || UNITY_IOS
        Handheld.Vibrate();
#else
        Debug.Log("Vibration triggered");
#endif
    }
}