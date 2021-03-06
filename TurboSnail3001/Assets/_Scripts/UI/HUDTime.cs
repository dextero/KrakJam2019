﻿using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HUDTime : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<TextMeshProUGUI>().SetText("0:00.000");
    }

    public static string TimeToString(float time) {
        int mins = (int)Mathf.Floor(time / 60.0f);
        int secs = (int)Mathf.Floor(time % 60.0f);
        int msecs = (int)((time % 1.0f) * 1000.0f);
        return string.Format("{0:D01}:{1:D02}.{2:D03}", mins, secs, msecs);
    }

    // Update is called once per frame
    void Update()
    {
        if (GameController.Instance.IsRunning) {
            float time = GameController.Instance.GameTime;
            GetComponent<TextMeshProUGUI>().SetText(TimeToString(time));
        }
    }
}
