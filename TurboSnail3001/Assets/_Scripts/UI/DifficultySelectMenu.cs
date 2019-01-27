using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DifficultySelectMenu : MonoBehaviour
{
    #region Inspector Variables
    [SerializeField, FoldoutGroup("UI")] private Button _Easy;
    [SerializeField, FoldoutGroup("UI")] private Button _Hard;
    [SerializeField, FoldoutGroup("UI")] private Button _Todo;
    [SerializeField, FoldoutGroup("UI")] private Button _Back;
    #endregion Inspector Variables

    #region Unity Methods
    private void Awake()
    {
        _Easy.onClick.AddListener(OnEasy);
        _Hard.onClick.AddListener(OnHard);
        _Todo.onClick.AddListener(OnTodo);
        _Back.onClick.AddListener(OnBack);
    }
    #endregion Unity Methods

    #region Private Methods
    private static void OnEasy()
    {
        GameController.SelectedTrack = TrackDifficulty.Easy;
        SceneManager.LoadScene(Settings.Instance.GameplaySceneIndex);
    }

    private static void OnHard()
    {
        GameController.SelectedTrack = TrackDifficulty.Hard;
        SceneManager.LoadScene(Settings.Instance.GameplaySceneIndex);
    }

    private static void OnTodo()
    {
        GameController.SelectedTrack = TrackDifficulty.Todo;
        SceneManager.LoadScene(Settings.Instance.GameplaySceneIndex);
    }
    private static void OnBack()
    {
        SceneManager.LoadScene(Settings.Instance.MenuSceneIndex);
    }
    #endregion Private Methods
}