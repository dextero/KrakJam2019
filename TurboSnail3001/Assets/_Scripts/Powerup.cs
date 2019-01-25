using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    void OnTriggerEnter(Collider other)
    {
        Snail snail = other.gameObject.GetComponentInParent<Snail>();
        if (snail != null) {
            snail.MoveSpeed = 10.0f;
            Destroy(this.gameObject);
        }
    }
}
