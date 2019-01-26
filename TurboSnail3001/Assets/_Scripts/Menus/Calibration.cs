using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Sirenix.OdinInspector;
using TMPro;
using TurboSnail3001.Input;
using UnityEngine;
using UnityEngine.SceneManagement;

internal class SampleSeries
{
    #region Public Variables
    public float MinValue { get; private set; }
    public float MaxValue { get; private set; }
    #endregion Public Variables

    #region Public Methods
    public SampleSeries(int numSamples,
                        HardwareController controller,
                        TextMeshProUGUI minText,
                        TextMeshProUGUI currText,
                        TextMeshProUGUI maxText)
    {
        _NumSamples = numSamples;
        _MinSamples = new List<int>();
        _MaxSamples = new List<int>();
        _Controller = controller;

        _MinText = minText;
        _CurrText = currText;
        _MaxText = maxText;
    }

    public void Update()
    {
        var value = _Controller.Frequency;
        _CurrText.SetText(value.ToString());

        _MinSamples.Add(value);
        _MinSamples.Sort();
        _MinSamples = _MinSamples.Take(_NumSamples).ToList();

        _MaxSamples.Add(value);
        _MaxSamples.Sort((a, b) => Math.Sign(b - a));
        _MaxSamples = _MaxSamples.Take(_NumSamples).ToList();

        MinValue = (float)_MinSamples.Average();
        MaxValue = (float)_MaxSamples.Average();

        _MinText.SetText(MinValue.ToString(CultureInfo.InvariantCulture));
        _MaxText.SetText(MaxValue.ToString(CultureInfo.InvariantCulture));
    }
    #endregion Public Methods

    #region Private Variables
    private readonly HardwareController _Controller;
    private readonly TextMeshProUGUI    _MinText;
    private readonly TextMeshProUGUI    _CurrText;
    private readonly TextMeshProUGUI    _MaxText;
    private readonly int                _NumSamples;
    private List<int>          _MinSamples;
    private List<int>          _MaxSamples;
    #endregion Private Variables
}

public class Calibration : SerializedMonoBehaviour
{
    #region Inspector Variables
    [SerializeField] private HardwareController LeftController;
    [SerializeField] private TextMeshProUGUI LeftMinValueText;
    [SerializeField] private TextMeshProUGUI LeftCurrentValueText;
    [SerializeField] private TextMeshProUGUI LeftMaxValueText;

    [SerializeField] private HardwareController RightController;
    [SerializeField] private TextMeshProUGUI RightMinValueText;
    [SerializeField] private TextMeshProUGUI RightCurrentValueText;
    [SerializeField] private TextMeshProUGUI RightMaxValueText;
    [SerializeField] private int NumSamples;

    [SerializeField] private int Collection = 1000;
    #endregion Inspector Variables

    #region Unity Methods
    private void Start()
    {
        LeftSamples  = new SampleSeries(NumSamples, LeftController, LeftMinValueText, LeftCurrentValueText, LeftMaxValueText);
        RightSamples = new SampleSeries(NumSamples, RightController, RightMinValueText, RightCurrentValueText, RightMaxValueText);
    }

    private void Update()
    {
        LeftSamples.Update();
        RightSamples.Update();

        _Count++;
        if (_Count > Collection)
        {
            /* setup settings */
            Settings.Instance.LeftControllerSettings.MinFrequency = (int)LeftSamples.MinValue;
            Settings.Instance.LeftControllerSettings.MaxFrequency = (int)LeftSamples.MaxValue;

            Settings.Instance.RightControllerSettings.MinFrequency = (int)RightSamples.MinValue;
            Settings.Instance.RightControllerSettings.MaxFrequency = (int)RightSamples.MaxValue;

            SceneManager.LoadScene(Settings.Instance.MenuSceneIndex);
        }
    }
    #endregion Unity Methods

    #region Private Variables
    private SampleSeries LeftSamples;
    private SampleSeries RightSamples;

    private int _Count;
    #endregion Private Variables
}