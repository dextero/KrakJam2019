using UnityEngine;

public class FaceCamera : MonoBehaviour
{
    private void Awake()
    {
        _Main = Camera.main;
    }

    private void Update()
    {
        transform.rotation = Quaternion.Euler(0.0f, _Main.transform.eulerAngles.y, 0.0f);
    }

    #region Private Variables
    private Camera _Main;
    #endregion Private Variables
}