using System.Threading;

namespace TurboSnail3001.Input
{
    using System;
    using System.Globalization;
    using System.IO.Ports;
    using Sirenix.OdinInspector;
    using UnityEngine;

    public class HardwareController : MonoBehaviour, IController
    {
        #region Public Variables
        public float State => _State;
        #endregion Public Variables

        #region Inspector Variables
        [Tooltip("The serial port where the Arduino is connected"), FoldoutGroup("Settings")]
        public string Port = "COM4";

        [Tooltip("The baudrate of the serial port"), FoldoutGroup("Settings")]
        public int Baudrate = 9600;

        [Tooltip("Controller frequency, when the hand is close to the controller")]
        [SerializeField, FoldoutGroup("Settings")]
        private int _MinFrequency = 1700000;

        [Tooltip("Controller frequency, when the hand is far away from the controller")]
        [SerializeField, FoldoutGroup("Settings")]
        private int _MaxFrequency = 1800000;

        [SerializeField, FoldoutGroup("Settings")]
        private int _CutoffFrequency = 1200000;
        #endregion Inspector Variables

        #region Unity Methods
        private void OnEnable()
        {
            _Stream = new SerialPort(Port, Baudrate);
            _Stream.Open();

            _Thread = new Thread(Read);
            _Thread.Start();
        }

        private void OnDisable()
        {
            _Stream.Close();
            _Thread.Abort();
        }

        private void Update()
        {
#if UNITY_EDITOR
            Sirenix.Utilities.Editor.GUIHelper.RequestRepaint();
#endif
            _State = 1.0f - Mathf.Clamp01((_Frequency - _MinFrequency) / (float)(_MaxFrequency - _MinFrequency));
        }
        #endregion Unity Methods

        #region Private Variables
        private SerialPort _Stream;
        private Thread _Thread;

        [ShowInInspector, ReadOnly, FoldoutGroup("Preview"), ProgressBar(0.0f, 1.0f)] private float _State;
        private int _Frequency;
        #endregion Private Variables

        #region Private Methods
        private void Read()
        {
            while (true)
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