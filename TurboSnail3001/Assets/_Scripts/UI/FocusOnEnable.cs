using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(TMP_InputField))]
public class FocusOnEnable : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnEnable()
    {
        // For some reason, ActivateInputField() does not work here?
        StartCoroutine(Focus());
    }

    private IEnumerator Focus() {
        yield return new WaitForSeconds(0);
        TMP_InputField input = GetComponent<TMP_InputField>();
        input.ActivateInputField();
    }
}