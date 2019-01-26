using System;

namespace TurboSnail3001
{
    using UnityEngine;

    [RequireComponent(typeof(Snail))]
    public class SnailInput : MonoBehaviour
    {
        #region Public Type
        [Serializable]
        public class SettingsData
        {
            public float Speed;
            public float RotateSpeed;
        }
        #endregion Public Type

        #region Inspector Variables
        [SerializeField] private SettingsData _Settings;
        #endregion Inspector Variables

        #region Unity Methods
        private void Awake()
        {
            _Snail = GetComponent<Snail>();

            _Transform = GetComponent<Transform>();
            _Rigidbody = GetComponent<Rigidbody>();
        }

        public void Start()
        {
            _Snail.Save.Settings = _Settings;
        }

        private void FixedUpdate()
        {
            /* get input */
            float left  = 0.0f;//_Input.LeftController.Position;
            float right = 0.0f;//_Input.RightController.Position;

            var input = GameController.Instance.InputSystem;

            /* on linux _Input is null when controllers are not plugged in */
            if (input != null)
            {
                left  = input.LeftController.Position;
                right = input.RightController.Position;
            }

            /* calculate velocity */
            float velocity = 1.0f;//(left +  right);

            /* todo: for debug only */
            if (UnityEngine.Input.GetKey(KeyCode.LeftArrow))
            {
                left = 0.0f;
                right = 1.0f;
            }
            if (UnityEngine.Input.GetKey(KeyCode.RightArrow))
            {
                left  = 1.0f;
                right = 0.0f;
            }

            /* calculate data for the ghost */
            var frame = new GhostSystem.FrameData
            {
                Frame = Time.frameCount,
                Left = left,
                Right = right,
                Velocity = velocity
            };
            _Snail.Save.Frames.Add(frame);

            /* apply forces */
            if (velocity > 0.0f) {
                _Rigidbody.AddForceAtPosition(_Transform.forward * _Settings.Speed * velocity, _Snail.Drivetrain.position);
            }
            if (left > 0.0f) {
                _Rigidbody.AddTorque(transform.up * left * _Settings.RotateSpeed);
            }
            if (right > 0.0f) {
                _Rigidbody.AddTorque(-transform.up * right * _Settings.RotateSpeed);
            }
        }
        #endregion Unity Methods

        #region Private Variables
        private Snail _Snail;
        private Rigidbody _Rigidbody;
        private Transform _Transform;
        #endregion Private Variables
    }
}