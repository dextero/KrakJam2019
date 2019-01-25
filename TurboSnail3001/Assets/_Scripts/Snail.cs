using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class Snail : MonoBehaviour
{
    private Rigidbody Body { get { return this.gameObject.GetComponent<Rigidbody>(); } }
    private Vector3 Velocity
    {
        get { return Body.velocity; }
        set { Body.velocity = value; }
    }
    private Transform Transform { get { return Body.transform; }}

    private Vector3 Position { get { return Body.position; } }
    private float Speed
    {
        get { return Velocity.magnitude; }
        set { Velocity = Transform.forward.normalized * value; }
    }

    private float MoveSpeed;
    private float RotationSpeed;

    // Start is called before the first frame update
    void Start()
    {
        Speed = 1.0f;
    }

    // Update is called once per frame
    [Button]
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.S)) { MoveSpeed = -1.0f; }
        if (Input.GetKeyDown(KeyCode.W)) { MoveSpeed = +1.0f; }
        if (Input.GetKeyDown(KeyCode.A)) { RotationSpeed = -1.0f; }
        if (Input.GetKeyDown(KeyCode.D)) { RotationSpeed = +1.0f; }
        if (Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.D)) { RotationSpeed = 0.0f; }

        Speed = MoveSpeed;
        Transform.Rotate(0.0f, RotationSpeed, 0.0f);
    }
}
