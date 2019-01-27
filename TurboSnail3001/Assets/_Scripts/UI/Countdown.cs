using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Countdown : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI Label;

    private IEnumerator UpdateLabel() {
        for (int i = 3; i > 0; --i) {
            Label.SetText(i.ToString());
            Label.transform.localScale = new Vector2(1.0f, 1.0f);
            yield return new WaitForSeconds(1);
        }

        Label.SetText("Go!");
        Label.transform.localScale = new Vector2(1.0f, 1.0f);
        GameController.Instance.StartGame();

        yield return new WaitForSeconds(1);
        this.gameObject.SetActive(false);
    }

    void OnEnable() {
        StartCoroutine(UpdateLabel());
    }
}
