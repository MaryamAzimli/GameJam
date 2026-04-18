using UnityEngine;
using System;

public class SettingsManager : MonoBehaviour
{
    public static SettingsManager Instance;

    private const string MUSIC_KEY = "music_on";
    private const string VIBRATION_KEY = "vibration_on";
    private const string LANGUAGE_KEY = "language";

    public bool IsMusicOn { get; private set; }
    public bool IsVibrationOn { get; private set; }
    public string CurrentLanguage { get; private set; }

    public Action<bool> OnMusicChanged;
    public Action<bool> OnVibrationChanged;
    public Action<string> OnLanguageChanged;

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

    private void LoadSettings()
    {
        IsMusicOn = PlayerPrefs.GetInt(MUSIC_KEY, 1) == 1;
        IsVibrationOn = PlayerPrefs.GetInt(VIBRATION_KEY, 1) == 1;
        CurrentLanguage = PlayerPrefs.GetString(LANGUAGE_KEY, "tr");
    }

    public void SetMusic(bool value)
    {
        IsMusicOn = value;
        PlayerPrefs.SetInt(MUSIC_KEY, value ? 1 : 0);
        PlayerPrefs.Save();

        OnMusicChanged?.Invoke(IsMusicOn);
    }

    public void ToggleMusic()
    {
        SetMusic(!IsMusicOn);
    }

    public void SetVibration(bool value)
    {
        IsVibrationOn = value;
        PlayerPrefs.SetInt(VIBRATION_KEY, value ? 1 : 0);
        PlayerPrefs.Save();

        OnVibrationChanged?.Invoke(IsVibrationOn);
    }

    public void ToggleVibration()
    {
        SetVibration(!IsVibrationOn);
    }

    public void SetLanguage(string languageCode)
    {
        CurrentLanguage = languageCode;
        PlayerPrefs.SetString(LANGUAGE_KEY, languageCode);
        PlayerPrefs.Save();

        OnLanguageChanged?.Invoke(CurrentLanguage);
    }
}