using UnityEngine;

public class SettingsManager : MonoBehaviour
{
    public static SettingsManager Instance;

    public bool musicOn = true;
    public bool soundOn = true;
    public bool vibrationOn = true;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            LoadSettings();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void ToggleMusic()
    {
        musicOn = !musicOn;
        SaveSettings();
        Debug.Log("Music: " + musicOn);
    }

    public void ToggleSound()
    {
        soundOn = !soundOn;
        SaveSettings();
        Debug.Log("Sound: " + soundOn);
    }

    public void ToggleVibration()
    {
        vibrationOn = !vibrationOn;
        SaveSettings();
        Debug.Log("Vibration: " + vibrationOn);
    }

    public void SaveSettings()
    {
        PlayerPrefs.SetInt("MusicOn", musicOn ? 1 : 0);
        PlayerPrefs.SetInt("SoundOn", soundOn ? 1 : 0);
        PlayerPrefs.SetInt("VibrationOn", vibrationOn ? 1 : 0);
        PlayerPrefs.Save();
    }

    public void LoadSettings()
    {
        musicOn = PlayerPrefs.GetInt("MusicOn", 1) == 1;
        soundOn = PlayerPrefs.GetInt("SoundOn", 1) == 1;
        vibrationOn = PlayerPrefs.GetInt("VibrationOn", 1) == 1;
    }
}