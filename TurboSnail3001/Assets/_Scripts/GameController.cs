using System.Collections;
using Sirenix.OdinInspector;
using TMPro;
using TurboSnail3001;
using TurboSnail3001.Input;
using UnityEngine;

public enum FinishResult
{
    Finished,
    Failed,
}

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

    // true if game countdown finished, snail still alive and not finished
    public bool IsRunning { get; set; }

    [FoldoutGroup("References")] public InputController InputSystem;
    [FoldoutGroup("References")] public GhostSystem GhostSystem;
    [FoldoutGroup("References")] public SaveSystem SaveSystem;
    [FoldoutGroup("References")] public Snail Target;
    [FoldoutGroup("References")] public GameObject NicknameInputOverlay;
    [FoldoutGroup("References")] public TextMeshProUGUI NicknamePromptMessage;
    [FoldoutGroup("References")] public TMP_InputField NicknameInput;
    [FoldoutGroup("References")] public Countdown Countdown;
    #endregion Public Variables

    #region Public Methods
    public int CalculateScore(FinishResult result) {
        switch (result) {
        case FinishResult.Finished:
            const float MAX_SCORE = 1000000.0f;
            const float MAX_EXPECTED_TIME_S = 120.0f;

            float timeElapsed = Time.fixedTime - _StartTime;
            float lerpFactor = timeElapsed / MAX_EXPECTED_TIME_S;
            return (int) Mathf.Lerp(MAX_SCORE, 0.0f, Mathf.Clamp01(lerpFactor));
        case FinishResult.Failed:
        default:
            return 0;
        }
    }
    public void StartGame() {
        IsRunning = true;
        _StartTime = Time.fixedTime;
    }
    public void Finish(FinishResult result) { 
        if (!IsRunning) {
            return;
        }

        IsRunning = false;
        Target.Save.Score = CalculateScore(result);
        NicknameInputOverlay.SetActive(true);
        NicknamePromptMessage.SetText($"Your score: {Target.Save.Score}");
    }
    public void OnNicknameInputFinished()
    {
        Target.Save.Nickname = NicknameInput.text;
        SaveSystem.Add(Target.Save);
        SaveSystem.Save();
        _GotoScene.GotoHighscore();
    }
    #endregion Public Methods

    #region Unity Methods
    private void Start() {
        _GameController = this;
        IsRunning = false;
        Countdown.gameObject.SetActive(true);
    }
    private void Awake() { _GotoScene = GetComponent<GotoScene>(); }
    #endregion Unity Methods

    #region Private Variables
    private static GameController _GameController;
    private GotoScene _GotoScene;

    private float _StartTime;
    #endregion Private Variables
}