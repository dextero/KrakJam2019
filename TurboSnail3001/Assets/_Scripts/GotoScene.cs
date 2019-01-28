using UnityEngine;
using UnityEngine.SceneManagement;

public class GotoScene : MonoBehaviour
{
    #region Public Methods
    public static void GotoMenu() { SceneManager.LoadScene(Settings.Instance.MenuSceneIndex); }
    public static void GotoGame() { SceneManager.LoadScene(Settings.Instance.GameplaySceneIndex); }
    public static void GotoHighscore() { SceneManager.LoadScene(Settings.Instance.HighscoreSceneIndex); }
    public static void GotoCalibration() { SceneManager.LoadScene(Settings.Instance.CalibrationSceneIndex); }
    public static void GotoDifficultySelectMenu() { SceneManager.LoadScene(Settings.Instance.DifficultySelectMenuSceneIndex); }
    #endregion Public Methods
}