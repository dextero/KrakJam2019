﻿using System;
using System.Collections;
using System.Collections.Generic;
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

public enum TrackDifficulty
{
    Easy,
    Hard,
    Todo,
}

[RequireComponent(typeof(GotoScene))]
public class GameController : MonoBehaviour
{
    #region Public Variables
    public static TrackDifficulty SelectedTrack = TrackDifficulty.Hard;
    private static string CurrentNickname = "Anonymous";

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

    public float GameTime { get { return Time.fixedTime - _StartTime; }}

    // true if game countdown finished, snail still alive and not finished
    public bool IsRunning { get; set; }

    [FoldoutGroup("References")] public InputController InputSystem;
    [FoldoutGroup("References")] public GhostSystem GhostSystem;
    [FoldoutGroup("References")] public SaveSystem SaveSystem;
    [FoldoutGroup("References")] public Snail Target;
    [FoldoutGroup("References")] public GameObject NicknameInputOverlay;
    [FoldoutGroup("References")] public TextMeshProUGUI NicknamePromptMessage;
    [FoldoutGroup("References")] public TMP_InputField NicknameInput;
    [FoldoutGroup("References")] public TextMeshProUGUI NicknameInputPlaceholder;
    [FoldoutGroup("References")] public Countdown Countdown;
    // NOTE: order of entries MUST match enum Track
    [FoldoutGroup("Tracks")] public List<GameObject> Tracks;
    #endregion Public Variables

    #region Public Methods
    public void StartGame() {
        IsRunning = true;
        _StartTime = Time.fixedTime;
    }
    public void Save() {
        Target.Save.Nickname = CurrentNickname;
        SaveSystem.Add(Target.Save);
        SaveSystem.Save();
    }
    public void Finish(FinishResult result) { 
        if (!IsRunning) {
            return;
        }

        IsRunning = false;
        Target.Save.Result = result;
        Target.Save.TimeElapsed = Time.fixedTime - _StartTime;
        NicknameInputPlaceholder.SetText(CurrentNickname);
        NicknameInputOverlay.SetActive(true);

        if (Target.Save.Finished) {
            NicknamePromptMessage.SetText($"Congratulations! Your time: {Target.Save.TimeElapsed}");
        } else {
            NicknamePromptMessage.SetText($"Your score: {Target.Save.Score}");
        }
    }
    public void OnNicknameInputFinished()
    {
        if (NicknameInput.text != "") {
            CurrentNickname = NicknameInput.text;
        }
        Save();
        GotoScene.GotoHighscore();
    }
    #endregion Public Methods

    #region Unity Methods
    private void Start() {
        if ((int) SelectedTrack >= Tracks.Count) {
            Debug.LogError($"invalid track id: {SelectedTrack} - not on Tracks list");
            GotoScene.GotoMenu();
        }
        for (int i = 0; i < Tracks.Count; ++i) {
            Tracks[i].SetActive(i == (int) SelectedTrack);
        }

        _GameController = this;
        IsRunning = false;
        Target.Save.Track = SelectedTrack;
        Countdown.gameObject.SetActive(true);
    }
    #endregion Unity Methods

    #region Private Variables
    private static GameController _GameController;

    private float _StartTime;
    #endregion Private Variables
}