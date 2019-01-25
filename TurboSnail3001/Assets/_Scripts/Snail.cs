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

    public float Speed
    {
        get { return Velocity.magnitude; }
        set { Velocity = Body.transform.forward.normalized * value; }
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    [Button]
    void Update()
    {
        Speed = 1.0f;
    }
}
