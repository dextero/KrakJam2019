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

    private const float SPEED_CHANGE_STEP = 1.0f;
    private const float UNBOOSTED_SPEED_LIMIT = 5.0f;
    private const float SLOWDOWN_FACTOR = 0.99f;
    private const float ROTATION_SPEED = 1.0f;

    public float MoveSpeed;
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
        if (Input.GetKeyDown(KeyCode.S)) { MoveSpeed -= SPEED_CHANGE_STEP; }
        if (Input.GetKeyDown(KeyCode.W)) { MoveSpeed += SPEED_CHANGE_STEP; }
        if (Input.GetKeyDown(KeyCode.A)) { RotationSpeed = -ROTATION_SPEED; }
        if (Input.GetKeyDown(KeyCode.D)) { RotationSpeed = +ROTATION_SPEED; }
        if (Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.D)) { RotationSpeed = 0.0f; }

        Speed = MoveSpeed;
        Transform.Rotate(0.0f, RotationSpeed, 0.0f);

        if (MoveSpeed > UNBOOSTED_SPEED_LIMIT || MoveSpeed < -UNBOOSTED_SPEED_LIMIT) {
            MoveSpeed *= SLOWDOWN_FACTOR;
        }
    }
}
