using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Canvas))]
public class KanseiDorifto : MonoBehaviour
{
    #region Inspector Variables
    [SerializeField] private GameObject _TrianglePrefab;
    [SerializeField] private int num_triangles = 20;
    [SerializeField] private float _AngleThreshold = 20.0f;
    [SerializeField] private float _SpeedThreshold = 0.0f;
    [SerializeField] private float _TimeThresholdSec = 3.0f;
    #endregion Inspector Variables

    #region Unity Methods
    private void Awake()
    {
        _Canvas = GetComponent<Canvas>();
    }

    private float lastFailedCheckTime = 0.0f;

    public void UpdateOverlay(Vector3 forward,
                              Vector3 velocity)
    {
        if (velocity.sqrMagnitude < _SpeedThreshold * _SpeedThreshold
                || Vector3.Angle(forward, velocity) < _AngleThreshold) {
            lastFailedCheckTime = Time.fixedTime;
            gameObject.SetActive(false);
        }

        gameObject.SetActive(lastFailedCheckTime + _TimeThresholdSec < Time.fixedTime);
    }

    private void Start()
    {
        var rect = _Canvas.pixelRect;

        float wh = rect.width / rect.height;
        float hw = rect.height / rect.width;

        axisScaleFactor = new Vector2(Math.Max(wh, 1.0f), Math.Max(hw, 1.0f));
        float minScreenDimension = Math.Min(rect.width, rect.height);
        minOffset = minScreenDimension / 3.0f;
        randomOffsetRange = (rect.width + rect.height) / 4 - minOffset;
    }

    private void Update()
    {
        ShuffleTriangles();
    }

    private void OnEnable() {
        for (int i = 0; i < num_triangles; ++i)
        {
            var triangle = Instantiate(_TrianglePrefab, Vector3.zero, Quaternion.identity);
            triangles.Add(triangle);
            triangle.transform.SetParent(transform, false);
        }
        ShuffleTriangles();
    }

    private void OnDisable() {
        foreach (GameObject triangle in triangles)
        {
            Destroy(triangle);
        }
        triangles.Clear();
    }
    #endregion Unity Methods

    #region Private Variables
    private readonly List<GameObject> triangles = new List<GameObject>();

    private float minOffset;
    private float randomOffsetRange;

    private Vector2 axisScaleFactor;
    private Canvas _Canvas;
    #endregion Private Variables

    private void ShuffleTriangles()
    {
        var rect   = _Canvas.pixelRect;

        System.Random random = new System.Random();
        foreach (GameObject triangle in triangles)
        {
            float angle = (float) (random.NextDouble() * 360.0);
            float angleRadians = angle * (float) Math.PI / 180.0f;
            float circleX = (float) Math.Cos(angleRadians);
            float circleY = (float) Math.Sin(angleRadians);
            Vector3 center = new Vector3(rect.width / 2,
                                         rect.height / 2,
                                         0.0f);
            Vector3 ellipse = new Vector3(circleX * minOffset * axisScaleFactor.x,
                                          circleY * minOffset * axisScaleFactor.y,
                                          0.0f);
            Vector3 randomOffset = new Vector3(circleX * (float) random.NextDouble() * randomOffsetRange,
                                               circleY * (float) random.NextDouble() * randomOffsetRange,
                                               0.0f);
            triangle.transform.SetPositionAndRotation(center + ellipse + randomOffset,
                                                      Quaternion.Euler(0.0f, 0.0f, angle));
        }
    }
}