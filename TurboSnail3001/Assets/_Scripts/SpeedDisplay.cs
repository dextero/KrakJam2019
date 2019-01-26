using TurboSnail3001;
using UnityEngine;

public class SpeedDisplay : MonoBehaviour
{
    [SerializeField]
    private GameObject snailObject;

    private Snail Snail
    {
        get { return snailObject.GetComponent<Snail>(); }
    }

    // Start is called before the first frame update
    private void Start()
    {
    }

    // Update is called once per frame
    private void Update()
    {
    }
}