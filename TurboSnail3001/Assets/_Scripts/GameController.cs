using System.Collections;
using UnityEngine;

[RequireComponent(typeof(GotoScene))]
public class GameController : MonoBehaviour
{
    #region Public Variables
    public static GameController Instance
    {
        get
        {
            if (_GameController == null)
            {
                _GameController = FindObjectOfType<GameController>();
            }

            return _GameController;
        }
    }
    #endregion Public Variables

    #region Public Methods
    public void Finish() { StartCoroutine(FinishCo()); }
    #endregion Public Methods

    #region Unity Methods
    private void Awake() { _GotoScene = GetComponent<GotoScene>(); }
    private void Start() { _GameController = this; }
    #endregion Unity Methods

    #region Private Variables
    private static GameController _GameController;
    private GotoScene _GotoScene;
    #endregion Private Variables

    #region Private Methods
    private IEnumerator FinishCo()
    {
        yield return new WaitForSeconds(1.0f);
        _GotoScene.GotoHighscore();
    }
    #endregion Private Methods
}