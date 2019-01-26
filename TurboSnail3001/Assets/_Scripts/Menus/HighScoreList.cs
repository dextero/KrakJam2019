using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public struct HighScoreEntry
{
    public string Nickname;
    public int Score;
}

public class HighScoreFile
{
    private static string HighScoreFilePath { get { return Path.Combine(Application.persistentDataPath, "hiscore.json"); } }
    public const int HIGH_SCORE_ENTRY_LIMIT = 10;

    public static List<HighScoreEntry> Load()
    {
        List<HighScoreEntry> scores = new List<HighScoreEntry>();
        try
        {
            Regex regex = new Regex(@"^(.*)\s+([0-9]+)$");
            foreach (string line in File.ReadAllLines(HighScoreFilePath))
            {
                Match match = regex.Match(line);
                if (match.Success)
                {
                    try
                    {
                        scores.Add(new HighScoreEntry
                        {
                            Nickname = match.Groups[1].Value,
                            Score = int.Parse(match.Groups[2].Value)
                        });
                    }
                    catch (FormatException e)
                    {
                        Debug.Log(e);
                    }
                }
            }
        }
        catch (IOException e)
        {
            Debug.Log(e);
        }
        return scores;
    }

    public static void Save(List<HighScoreEntry> scores)
    {
        try
        {
            Directory.CreateDirectory(Path.GetDirectoryName(HighScoreFilePath));
            string scoresString = "";
            foreach (HighScoreEntry entry in scores)
            {
                scoresString += string.Format("{0} {1}\n", entry.Nickname, entry.Score);
            }
            File.WriteAllText(HighScoreFilePath, scoresString);
        }
        catch (IOException e)
        {
            Debug.Log(e);
        }
    }

    public bool IsHighScore(int score)
    {
        return Load().Any(entry => entry.Score < score);
    }

    public static void Update(string newNickname, int newScore)
    {
        List<HighScoreEntry> scores = Load();
        scores.Add(new HighScoreEntry { Nickname = newNickname, Score = newScore });
        Save(scores.OrderBy(entry => -entry.Score)
                   .Take(HIGH_SCORE_ENTRY_LIMIT)
                   .ToList());
    }
}

public class HighScoreList : MonoBehaviour
{
    [SerializeField]
    private GameObject HighScoreListEntryPrefab;

    private GameObject MakeHighScoreListEntryWithText(string text)
    {
        GameObject entry = Instantiate(HighScoreListEntryPrefab, Vector3.zero, Quaternion.identity);
        entry.GetComponent<TextMeshProUGUI>().SetText(text);
        entry.GetComponent<RectTransform>().sizeDelta = new Vector2Int(1920, 70);
        return entry;
    }

    private GameObject MakeHighScoreHeader()
    {
        GameObject entry = MakeHighScoreListEntryWithText("High scores");
        entry.GetComponent<TextMeshProUGUI>().fontStyle |= FontStyles.Bold;
        return entry;
    }

    private GameObject MakeHighScoreListEntry(string nickname, int score)
    {
        return MakeHighScoreListEntryWithText(string.Format("{0}: {1}", nickname, score));
    }

    public void AppendEntry(GameObject entry)
    {
        entry.transform.SetParent(transform, false);
    }

    public void AppendEmptyEntry()
    {
        AppendEntry(MakeHighScoreListEntryWithText(""));
    }

    private void Reload() {
        foreach (Transform childTransform in transform)
        {
            Destroy(childTransform.gameObject);
        }

        List<HighScoreEntry> entries = HighScoreFile.Load();

        AppendEmptyEntry();
        AppendEntry(MakeHighScoreHeader());
        foreach (HighScoreEntry entry in entries)
        {
            AppendEntry(MakeHighScoreListEntry(entry.Nickname, entry.Score));
        }
        for (int i = 0; i < HighScoreFile.HIGH_SCORE_ENTRY_LIMIT - entries.Count + 1; ++i) {
            AppendEmptyEntry();
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        Reload();
    }

    [Button]
    void AddHighScore(string nickname, int score)
    {
        HighScoreFile.Update(nickname, score);
        Reload();
    }

    [Button]
    void ResetHighScores()
    {
        HighScoreFile.Save(new List<HighScoreEntry>());
        Reload();
    }

    // Update is called once per frame
    void Update()
    {
    }
}
