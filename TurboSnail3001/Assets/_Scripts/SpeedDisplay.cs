using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SpeedDisplay : MonoBehaviour
{
    [SerializeField]
    private GameObject snailObject;

    private Snail Snail {
        get { return snailObject.GetComponent<Snail>(); }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        GetComponent<TextMeshProUGUI>().SetText(string.Format("{0:0.0} fasts/s", Snail.MoveSpeed));
    }
}
