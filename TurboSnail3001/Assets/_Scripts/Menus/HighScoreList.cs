using System;
using System.Linq;
using TMPro;
using UnityEngine;

public class HighScoreList : MonoBehaviour
{
    #region Public Methods
    public void AppendEntry(GameObject entry, Save save)
    {
        switch (save.Track)
        {
            case TrackDifficulty.Easy:
                entry.transform.SetParent(_EasyContainer.transform, false);
                break;
            case TrackDifficulty.Hard:
                entry.transform.SetParent(_HardContainer.transform, false);
                break;
            case TrackDifficulty.Todo:
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
       
    }
    #endregion Public Methods

    #region Inspector Variables
    [SerializeField]
    private GameObject _HighScoreListEntryPrefab;

    [SerializeField] private GameObject _EasyContainer;
    [SerializeField] private GameObject _HardContainer;

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
        return MakeHighScoreListEntryWithText(save.ToString());
    }

    private void Reload()
    {
        foreach (Transform childTransform in transform)
        {
            Destroy(childTransform.gameObject);
        }

        var entries = _SaveSystem.Load();
        var sorted = entries.Saves.OrderByDescending(x => x);
        foreach (var entry in sorted)
        {
            AppendEntry(MakeHighScoreListEntry(entry), entry);
        }
    }
    #endregion Private Methods
}