using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KanseiDorifto : MonoBehaviour
{
    [SerializeField]
    private GameObject TrianglePrefab;

    private List<GameObject> triangles = new List<GameObject>();

    private const int NUM_TRIANGLES = 20;
    private float minOffset;
    private float randomOffsetRange;

    private Vector2 axisScaleFactor;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < NUM_TRIANGLES; ++i) {
            GameObject triangle = Instantiate(TrianglePrefab, Vector3.zero, Quaternion.identity);
            triangles.Add(triangle);
            triangle.transform.SetParent(transform, false);
        }

        float wh = (float) Screen.currentResolution.width / (float) Screen.currentResolution.height;
        float hw = (float) Screen.currentResolution.height / (float) Screen.currentResolution.width;
        axisScaleFactor = new Vector2(Math.Max(wh, 1.0f), Math.Max(hw, 1.0f));
        int minScreenDimension = Math.Min(Screen.currentResolution.width, Screen.currentResolution.height);
        minOffset = (float) minScreenDimension / 3.0f;
        randomOffsetRange = (Screen.currentResolution.width + Screen.currentResolution.height) / 4 - minOffset;
    }

    // Update is called once per frame
    void Update()
    {
        System.Random random = new System.Random();
        foreach (GameObject triangle in triangles) {
            float angle = (float) (random.NextDouble() * 360.0);
            float angleRadians = angle * (float) Math.PI / 180.0f;
            float circleX = (float) Math.Cos(angleRadians);
            float circleY = (float) Math.Sin(angleRadians);
            Vector3 center = new Vector3(Screen.currentResolution.width / 2,
                                         Screen.currentResolution.height / 2,
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
