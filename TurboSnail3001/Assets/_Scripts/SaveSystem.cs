using System;
using System.Collections.Generic;
using System.IO;
using Sirenix.OdinInspector;
using TurboSnail3001;
using UnityEngine;

[Serializable]
public class Save
{
    public string Nickname;

    public Vector3 StartPosition;
    public Quaternion StartRotation;

    public SnailInput.SettingsData Settings;
    public List<GhostSystem.FrameData> Frames = new List<GhostSystem.FrameData>();
}

public class SaveSystem : MonoBehaviour
{
    #region Public Types
    [Serializable]
    public class SaveData
    {
        public List<Save> Saves;
    }
    #endregion Public Types

    #region Public Methods
    public void Add(Save save)
    {
        if (_SaveData == null) { Load(); }
        _SaveData.Saves.Add(save);
    }

    [Button]
    public SaveData Load()
    {
        if (_SaveData != null) { return _SaveData; }

        var json = File.ReadAllText(SavesDataPath);
        _SaveData = JsonUtility.FromJson<SaveData>(json);

        return _SaveData;
    }

    [Button]
    public void Save()
    {
        if (_SaveData == null) { Load(); }

        var json = JsonUtility.ToJson(_SaveData);
        File.WriteAllText(SavesDataPath, json);
    }
    [Button]
    public void Clear()
    {
        var json = JsonUtility.ToJson(new SaveData());
        File.WriteAllText(SavesDataPath, json);
    }
    #endregion Public Methods

    #region Inspector Variables
    [ShowInInspector] private SaveData _SaveData;
    #endregion Inspector Variables

    #region Unity Methods
    private void Awake() { Load(); }
    #endregion Unity Methods

    #region Private Variables
    private static string SavesDataPath => Path.Combine(Application.persistentDataPath, "snejvs.json");
    #endregion Private Variables
}