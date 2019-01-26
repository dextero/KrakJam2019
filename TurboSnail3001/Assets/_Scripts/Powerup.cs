using TurboSnail3001;
using UnityEngine;

public class Powerup : MonoBehaviour
{
    // Start is called before the first frame update
    private void Start()
    {
    }

    // Update is called once per frame
    private void Update()
    {
    }

    private void OnTriggerEnter(Collider other)
    {
        Snail snail = other.gameObject.GetComponentInParent<Snail>();
        if (snail != null)
        {
            Destroy(this.gameObject);
        }
    }
}