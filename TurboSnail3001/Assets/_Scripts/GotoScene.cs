using UnityEngine;
using UnityEngine.SceneManagement;

public class GotoScene : MonoBehaviour
{
    #region Public Methods
    public void GotoMenu() { SceneManager.LoadScene(Settings.Instance.MenuSceneIndex); }
    public void GotoGame() { SceneManager.LoadScene(Settings.Instance.GameplaySceneIndex); }
    public void GotoHighscore() { SceneManager.LoadScene(Settings.Instance.HighscoreSceneIndex); }
    public void GotoCalibration() { SceneManager.LoadScene(Settings.Instance.CalibrationSceneIndex); }
    public void GotoDifficultySelectMenu() { SceneManager.LoadScene(Settings.Instance.DifficultySelectMenuSceneIndex); }
    #endregion Public Methods
}