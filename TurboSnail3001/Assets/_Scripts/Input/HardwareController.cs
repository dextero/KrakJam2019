﻿using System.Threading;

namespace TurboSnail3001.Input
{
    using System;
    using System.Globalization;
    using System.IO.Ports;
    using Sirenix.OdinInspector;
    using UnityEngine;

    public class HardwareController : MonoBehaviour
    {
        #region Public Variables
        public float State => _State;
        public int Frequency => _Frequency;

        [Tooltip("The serial port where the Arduino is connected"), FoldoutGroup("Settings")]
        public string Port = "COM4";

        [Tooltip("The baudrate of the serial port"), FoldoutGroup("Settings")]
        public int Baudrate = 9600;

        [Tooltip("Controller frequency, when the hand is close to the controller"), FoldoutGroup("Settings")]
        public int MinFrequency = 1700000;

        [Tooltip("Controller frequency, when the hand is far away from the controller"), FoldoutGroup("Settings")]
        public int MaxFrequency = 1800000;
        #endregion Public Variables

        #region Inspector Variables
        [SerializeField, FoldoutGroup("Settings")]
        private int _CutoffFrequency = 1200000;
        #endregion Inspector Variables

        #region Unity Methods
        private void OnEnable()
        {
            try
            {
                _Stream = new SerialPort(Port, Baudrate);
                _Stream.Open();
            }
            catch (Exception e)
            {
                Debug.LogError("No controllero");
            }

            _Thread = new Thread(Read);
            _Thread.Start();
        }

        private void OnDisable()
        {
            _ThreadRun = false;
            _Thread.Join();

            _Stream.Close();
        }

        private void Update()
        {
#if UNITY_EDITOR
            Sirenix.Utilities.Editor.GUIHelper.RequestRepaint();
#endif
            _State = 1.0f - Mathf.Clamp01((_Frequency - MinFrequency) / (float)(MaxFrequency - MinFrequency));
        }
        #endregion Unity Methods

        #region Private Variables
        private SerialPort _Stream;
        private Thread _Thread;

        [ShowInInspector, ReadOnly, FoldoutGroup("Preview"), ProgressBar(0.0f, 1.0f)] private float _State;
        private int _Frequency;
        private bool _ThreadRun = true;
        #endregion Private Variables

        #region Private Methods
        private void Read()
        {
            if (_Stream == null || !_Stream.IsOpen) { return; }
            while (_ThreadRun)
            {
                try
                {
                    var data = _Stream.ReadLine();
                    int.TryParse(data, NumberStyles.Integer, NumberFormatInfo.InvariantInfo, out int frequency);
                    if (frequency > _CutoffFrequency)
                    {
                        _Frequency = frequency;
                    }
                }
                catch (TimeoutException)
                {
                }
            }
        }
        #endregion Private Methods
    }
}