using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    #region Inspector Variables
    [SerializeField, FoldoutGroup("UI")] private Button _Start;
    [SerializeField, FoldoutGroup("UI")] private Button _Highscore;
    [SerializeField, FoldoutGroup("UI")] private Button _Calibration;
    [SerializeField, FoldoutGroup("UI")] private Button _Exit;
    #endregion Inspector Variables

    #region Unity Methods
    private void Awake()
    {
        _Start.onClick.AddListener(OnStart);
        _Highscore.onClick.AddListener(OnHighscore);
        _Calibration.onClick.AddListener(OnCalibration);
        _Exit.onClick.AddListener(OnExit);
    }
    #endregion Unity Methods

    #region Private Methods
    private void OnStart()
    {
    }

    private void OnHighscore()
    {
    }

    private void OnCalibration()
    {
    }
    private void OnExit()
    {
        Application.Quit();
    }
    #endregion Private Methods
}