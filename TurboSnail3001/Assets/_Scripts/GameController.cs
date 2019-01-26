using System.Collections;
using Sirenix.OdinInspector;
using TMPro;
using TurboSnail3001;
using TurboSnail3001.Input;
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

    [FoldoutGroup("References")] public InputController InputSystem;
    [FoldoutGroup("References")] public GhostSystem GhostSystem;
    [FoldoutGroup("References")] public SaveSystem SaveSystem;
    [FoldoutGroup("References")] public Snail Target;
    [FoldoutGroup("References")] public GameObject NicknameInputOverlay;
    [FoldoutGroup("References")] public TextMeshProUGUI NicknamePromptMessage;
    [FoldoutGroup("References")] public TMP_InputField NicknameInput;
    #endregion Public Variables

    #region Public Methods
    public int CalculateScore() {
        const float MAX_SCORE = 1000000.0f;
        const float MAX_EXPECTED_TIME_S = 120.0f;

        float timeElapsed = Time.fixedTime - _StartTime;
        float lerpFactor = timeElapsed / MAX_EXPECTED_TIME_S;
        return (int) Mathf.Lerp(MAX_SCORE, 0.0f, Mathf.Clamp01(lerpFactor));
    }
    public void Finish() { 
        Target.Save.Score = CalculateScore();
        NicknameInputOverlay.SetActive(true);
        NicknamePromptMessage.SetText($"Your score: {Target.Save.Score}");
    }
    public void OnNicknameInputFinished()
    {
        SaveSystem.Add(Target.Save);
        SaveSystem.Save();
        _GotoScene.GotoHighscore();
    }
    #endregion Public Methods

    #region Unity Methods
    private void Awake() { _GotoScene = GetComponent<GotoScene>(); }
    private void Start() {
        _GameController = this;
        _StartTime = Time.fixedTime;
    }
    #endregion Unity Methods

    #region Private Variables
    private static GameController _GameController;
    private GotoScene _GotoScene;

    private float _StartTime;
    #endregion Private Variables
}