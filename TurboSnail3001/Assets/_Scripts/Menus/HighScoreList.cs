using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

[Serializable]
public struct HighScoreEntry
{
    public string Nickname;
    public int Score;
}

[Serializable]
public class Highscores
{
    public List<HighScoreEntry> Entries;
}

public static class HighScoreFile
{
    #region Public Methods
    public static List<HighScoreEntry> Load()
    {
        var json = File.ReadAllText(HighScoreFilePath);
        var result = JsonUtility.FromJson<Highscores>(json);

        return result.Entries;
    }

    public static void Save(List<HighScoreEntry> scores)
    {
        var obj = new Highscores {Entries = scores};
        var json = JsonUtility.ToJson(obj);

        File.WriteAllText(HighScoreFilePath, json);
    }

    public static void Update(string newNickname, int newScore)
    {
        var scores = Load();
        scores.Add(new HighScoreEntry { Nickname = newNickname, Score = newScore });
        Save(scores.OrderBy(entry => -entry.Score)
                   .ToList());
    }
    #endregion Public Methods

    #region Private Variables
    private static string HighScoreFilePath => Path.Combine(Application.persistentDataPath, "hiscore.json");
    #endregion Private Variables
}

public class HighScoreList : MonoBehaviour
{
    #region Public Methods
    public void AppendEntry(GameObject entry)
    {
        entry.transform.SetParent(transform, false);
    }
    #endregion Public Methods

    #region Inspector Variables
    [SerializeField]
    private GameObject _HighScoreListEntryPrefab;
    #endregion Inspector Variables

    #region Unity Methods
    private void Start()
    {
        Reload();
    }
    #endregion Unity Methods

    #region Private Methods
    private GameObject MakeHighScoreListEntryWithText(string text)
    {
        var entry = Instantiate(_HighScoreListEntryPrefab, Vector3.zero, Quaternion.identity);
        entry.GetComponent<TextMeshProUGUI>().SetText(text);
        return entry;
    }

    private GameObject MakeHighScoreListEntry(string nickname, int score)
    {
        return MakeHighScoreListEntryWithText($"{nickname}: {score}");
    }

    private void Reload()
    {
        foreach (Transform childTransform in transform)
        {
            Destroy(childTransform.gameObject);
        }

        var entries = HighScoreFile.Load();
        foreach (var entry in entries)
        {
            AppendEntry(MakeHighScoreListEntry(entry.Nickname, entry.Score));
        }
    }
    #endregion Private Methods

    #region Helper Methods
    [Button]
    private void AddHighScore(string nickname, int score)
    {
        HighScoreFile.Update(nickname, score);
        Reload();
    }

    [Button]
    private void ResetHighScores()
    {
        HighScoreFile.Save(new List<HighScoreEntry>());
        Reload();
    }
    #endregion Helper Methods
}