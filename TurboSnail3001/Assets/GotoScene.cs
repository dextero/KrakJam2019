using UnityEngine;
using UnityEngine.SceneManagement;

public class GotoScene : MonoBehaviour
{
    #region Public Methods
    public void GotoMenu() { SceneManager.LoadScene(Settings.Instance.MenuSceneIndex); }
    #endregion Public Methods
}