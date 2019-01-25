using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class Snail : MonoBehaviour
{
    private Vector3 Velocity {
        get {
            return this.gameObject.GetComponent<Rigidbody>().velocity;
        }
        set {
            this.gameObject.GetComponent<Rigidbody>().velocity = value;
        }
    }

    public float Speed {
        get {
            return Velocity.magnitude;
        }
        set {
            Velocity = Velocity.normalized * value;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    [Button]
    void Update()
    {
    }
}
