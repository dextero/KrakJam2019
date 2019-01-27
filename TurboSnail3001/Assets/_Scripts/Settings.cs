using System;
using Sirenix.OdinInspector;
using UnityEngine;

public class Settings : MonoBehaviour
{
    #region Public Types
    [Serializable]
    public class ControllerSettings
    {
        public int MinFrequency;
        public int MaxFrequency;
    }
    #endregion Public Types

    #region Public Variables
    public static Settings Instance
    {
        get
        {
            if (_Settings == null)
            {
                _Settings = FindObjectOfType<Settings>();
                if (_Settings != null) { DontDestroyOnLoad(_Settings.gameObject); }
            }
            if (_Settings == null)
            {
                Resources.LoadAll<Settings>("");
                var prefab = Resources.FindObjectsOfTypeAll<Settings>()[0];
                _Settings = Instantiate(prefab);
                _Settings.name = "Settings";
                if (_Settings != null) { DontDestroyOnLoad(_Settings.gameObject); }
            }

            return _Settings;
        }
    }

    [FoldoutGroup("Controllers")] public ControllerSettings LeftControllerSettings;
    [FoldoutGroup("Controllers")] public ControllerSettings RightControllerSettings;

    [FoldoutGroup("Scenes")] public int GameplaySceneIndex;
    [FoldoutGroup("Scenes")] public int MenuSceneIndex;
    [FoldoutGroup("Scenes")] public int HighscoreSceneIndex;
    [FoldoutGroup("Scenes")] public int CalibrationSceneIndex;
    [FoldoutGroup("Scenes")] public int DifficultySelectMenuSceneIndex;
    #endregion Public Variables

    #region Unity Methods
    private void Awake()
    {
        _Settings = this;
        DontDestroyOnLoad(gameObject);
    }

    private void OnEnable()
    {
        if (PlayerPrefs.HasKey(_LeftMin)) { LeftControllerSettings.MinFrequency   = PlayerPrefs.GetInt(_LeftMin); }
        if (PlayerPrefs.HasKey(_LeftMax)) { LeftControllerSettings.MaxFrequency   = PlayerPrefs.GetInt(_LeftMax); }

        if (PlayerPrefs.HasKey(_RightMin)) { RightControllerSettings.MinFrequency = PlayerPrefs.GetInt(_RightMin); }
        if (PlayerPrefs.HasKey(_RightMax)) { RightControllerSettings.MaxFrequency = PlayerPrefs.GetInt(_RightMax); }
    }
    private void OnDisable()
    {
        PlayerPrefs.SetInt(_LeftMin, LeftControllerSettings.MinFrequency);
        PlayerPrefs.SetInt(_LeftMax, LeftControllerSettings.MaxFrequency);

        PlayerPrefs.SetInt(_RightMin, RightControllerSettings.MinFrequency);
        PlayerPrefs.SetInt(_RightMax, RightControllerSettings.MaxFrequency);
    }
    #endregion Unity Methods

    #region Private Variables
    private static Settings _Settings;

    private const string _LeftMin  = "_LeftMin";
    private const string _LeftMax  = "_LeftMax";
    private const string _RightMin = "_RightMin";
    private const string _RightMax = "_RightMax";
    #endregion Private Variables
}