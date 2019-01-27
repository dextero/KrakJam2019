using TurboSnail3001;
using UnityEngine;

[RequireComponent(typeof(Snail))]
public class GhostInput : MonoBehaviour
{
    #region Public Methods
    public void Initialize(Save save)
    {
        _Save = save;

        transform.position = _Save.StartPosition;
        transform.rotation = _Save.StartRotation;
    }
    #endregion Public Methods

    #region Unity Methods
    private void Awake()
    {
        _Snail = GetComponent<Snail>();
        _Transform = GetComponent<Transform>();
        _Rigidbody = GetComponent<Rigidbody>();
    }
    private void FixedUpdate()
    {
        if (!GameController.Instance.IsRunning) {
            return;
        }

        if (_Frame < _Save.Frames.Count)
        {
            var frame = _Save.Frames[_Frame];

            var velocity = frame.Velocity;
            var left = frame.Left;
            var right = frame.Right;

            if (!float.IsNaN(_Save.Settings.Speed) && !float.IsNaN(velocity))
            {
                _Rigidbody.AddForceAtPosition(_Transform.forward * _Save.Settings.Speed * velocity, _Snail.Drivetrain.position);

                _Rigidbody.AddTorque(transform.up * left   * _Save.Settings.RotateSpeed);
                _Rigidbody.AddTorque(-transform.up * right * _Save.Settings.RotateSpeed);
            }
            _Frame++;
        }
    }
    #endregion Unity Methods

    #region Private Variables
    public Save _Save;

    private int _Frame;
    private Transform _Transform;
    private Rigidbody _Rigidbody;
    private Snail _Snail;
    #endregion Private Variables
}