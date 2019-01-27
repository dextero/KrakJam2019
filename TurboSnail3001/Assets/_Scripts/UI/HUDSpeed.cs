using System.Collections;
using System.Collections.Generic;
using TMPro;
using TurboSnail3001;
using UnityEngine;

public class HUDSpeed : MonoBehaviour
{
    [SerializeField] private Snail Snail;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        GetComponent<TextMeshProUGUI>().SetText(string.Format("{0:0.0} fasts/h", Snail.Speed));
    }
}
