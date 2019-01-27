using System;
using System.Linq;
using TMPro;
using UnityEngine;

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

    [SerializeField] private SaveSystem _SaveSystem;
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

    private GameObject MakeHighScoreListEntry(Save save)
    {
        return MakeHighScoreListEntryWithText($"{save.Nickname}: {save.Score} ({save.Track})");
    }

    private void Reload()
    {
        foreach (Transform childTransform in transform)
        {
            Destroy(childTransform.gameObject);
        }

        var entries = _SaveSystem.Load();
        var sorted = entries.Saves.OrderByDescending(x => x.Score);
        foreach (var entry in sorted)
        {
            AppendEntry(MakeHighScoreListEntry(entry));
        }
    }
    #endregion Private Methods
}