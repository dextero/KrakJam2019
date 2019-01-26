using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using TurboSnail3001.Input;
using Sirenix.OdinInspector;

class SampleSeries
{
    private IController _Controller;
    private TextMeshProUGUI _MinText;
    private TextMeshProUGUI _CurrText;
    private TextMeshProUGUI _MaxText;
    private int _NumSamples;
    private List<float> _MinSamples; 
    private List<float> _MaxSamples; 
    public float MinValue { get; private set; }
    public float MaxValue { get; private set; }

    public SampleSeries(int numSamples,
                        IController controller,
                        TextMeshProUGUI minText,
                        TextMeshProUGUI currText,
                        TextMeshProUGUI maxText)
    {
        _NumSamples = numSamples;
        _MinSamples = new List<float>();
        _MaxSamples = new List<float>();
        _Controller = controller;
        _MinText = minText;
        _CurrText = currText;
        _MaxText = maxText;
    }

    public void Update()
    {
        float value = _Controller.State;
        _CurrText.SetText(value.ToString());

        _MinSamples.Add(value);
        _MinSamples.Sort();
        _MinSamples = _MinSamples.Take(_NumSamples).ToList();
        MinValue = _MinSamples.Average();
        _MinText.SetText(MinValue.ToString());

        _MaxSamples.Add(value);
        _MaxSamples.Sort((a, b) => Math.Sign(b - a));
        _MaxSamples = _MaxSamples.Take(_NumSamples).ToList();
        MaxValue = _MaxSamples.Average();
        _MaxText.SetText(MaxValue.ToString());
    }
}

public class Calibration : SerializedMonoBehaviour
{
    [SerializeField] private IController LeftController;
    [SerializeField] private TextMeshProUGUI LeftMinValueText;
    [SerializeField] private TextMeshProUGUI LeftCurrentValueText;
    [SerializeField] private TextMeshProUGUI LeftMaxValueText;
    [SerializeField] private IController RightController;
    [SerializeField] private TextMeshProUGUI RightMinValueText;
    [SerializeField] private TextMeshProUGUI RightCurrentValueText;
    [SerializeField] private TextMeshProUGUI RightMaxValueText;
    [SerializeField] private int NumSamples;

    SampleSeries LeftSamples;
    SampleSeries RightSamples;

    // Start is called before the first frame update
    void Start()
    {
        LeftSamples = new SampleSeries(NumSamples, LeftController, LeftMinValueText, LeftCurrentValueText, LeftMaxValueText);
        RightSamples = new SampleSeries(NumSamples, RightController, RightMinValueText, RightCurrentValueText, RightMaxValueText);
    }

    // Update is called once per frame
    void Update()
    {
        LeftSamples.Update();
        RightSamples.Update();
    }
}
