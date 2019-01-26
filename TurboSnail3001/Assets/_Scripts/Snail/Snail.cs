using UnityEngine;

public class Snail : MonoBehaviour
{
    #region Public Types
    public enum ControlSignals
    {
        RightPull,
        RightPush,

        LeftPull,
        LeftPush,

        BothPull,
        BothPush
    }
    #endregion Public Types

    #region Public Methods
    public void Left(float value)
    {
        _Rigidbody.AddTorque(0.0f, -_RotateSpeed * Mathf.Clamp01(1.0f - value), 0.0f);
    }
    #endregion Public Methods

    #region Inspector Variables
    [SerializeField] private float _Speed;
    [SerializeField] private float _RotateSpeed;
    #endregion Inspector Variables

    #region Unity Methods
    private void Awake()
    {
        _Transform = GetComponent<Transform>();
        _Rigidbody = GetComponent<Rigidbody>();
    }

    private void Update()
    {
    }

    private void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.UpArrow))
        {
            _Rigidbody.AddForce(_Transform.forward * _Speed);
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            _Rigidbody.AddForce(-_Transform.forward * _Speed);
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            _Rigidbody.AddTorque(0.0f, -_RotateSpeed, 0.0f);
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            _Rigidbody.AddTorque(0.0f, _RotateSpeed, 0.0f);
        }
    }
    #endregion Unity Methods

    #region Private Variables
    private Rigidbody _Rigidbody;
    private Transform _Transform;
    #endregion Private Variables
}