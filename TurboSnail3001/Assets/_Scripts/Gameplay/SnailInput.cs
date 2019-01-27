using System;

namespace TurboSnail3001
{
    using UnityEngine;

    [RequireComponent(typeof(Snail))]
    public class SnailInput : MonoBehaviour
    {
        public static bool IsLinux
        {
            get
            {
                // https://stackoverflow.com/a/5117005/2339636
                int p = (int)Environment.OSVersion.Platform;
                return (p == 4) || (p == 6) || (p == 128);
            }
        }

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
        [SerializeField] private float _EndOfTheWorldY = -50.0f;
        [SerializeField] private KanseiDorifto _DriftOverlay;
        [SerializeField] private float _BonusTime = 2.0f;
        [SerializeField] private float _BonusMultiplier = 2.0f;

        [SerializeField] private AudioSource _Drift;
        [SerializeField] private AudioSource _Staedy;
        [SerializeField] private AudioSource _Intro;
        [SerializeField] public AudioSource _Bang;
        [SerializeField] public AudioSource _Bang2;

        [SerializeField] private float _MusicVelocityScale = 90.0f;
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

            _Drift.volume = 1.0f;
            _Staedy.Play();
        }
        private void FixedUpdate()
        {
            if (!GameController.Instance.IsRunning)
            {
                return;
            }

            /* get input */
            float left = 0.0f;
            float right = 0.0f;

            var input = GameController.Instance.InputSystem;

            if (IsLinux)
            {
                /* on linux _Input is null when controllers are not plugged in */
                if (float.IsNaN(input.LeftController.Position) || float.IsNaN(input.RightController.Position))

                {
                    if (UnityEngine.Input.GetKey(KeyCode.LeftArrow))
                    {
                        right = 1.0f;
                    }
                    if (UnityEngine.Input.GetKey(KeyCode.RightArrow))
                    {
                        left = 1.0f;
                    }
                }
                else
                {
                    left = input.LeftController.Position;
                    right = input.RightController.Position;
                }
            }
            else
            {
                left = input.LeftController.Position;
                right = input.RightController.Position;

                if (UnityEngine.Input.GetKey(KeyCode.LeftArrow))
                {
                    right = 1.0f;
                    left = 0.0f;
                }
                if (UnityEngine.Input.GetKey(KeyCode.RightArrow))
                {
                    left = 1.0f;
                    right = 0.0f;
                }
            }

            /* calculate velocity */
            float velocity = (left + right);

            if(_DriftOverlay.Activated)
            {
                _DriftOverlay.Activated = false;
                _Drift.Play();
                _Countdown = _BonusTime;
            }

            _Countdown -= Time.fixedDeltaTime;
            if(_Countdown > 0.0f)
            {
                velocity *= _BonusMultiplier;
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
            _Rigidbody.AddForceAtPosition(_Transform.forward * _Settings.Speed * velocity, _Snail.Drivetrain.position);
            _Rigidbody.AddTorque(transform.up * left * _Settings.RotateSpeed);
            _Rigidbody.AddTorque(-transform.up * right * _Settings.RotateSpeed);

            if (_Transform.position.y < _EndOfTheWorldY)
            {
                GameController.Instance.Finish(FinishResult.Failed);
            }

            _Staedy.volume = 0.2f + _Rigidbody.velocity.magnitude / _MusicVelocityScale; 

            _DriftOverlay.UpdateOverlay(_Transform.forward, _Rigidbody.velocity);
        }
        #endregion Unity Methods

        #region Private Variables
        private Snail _Snail;
        private Rigidbody _Rigidbody;
        private Transform _Transform;

        private float _Countdown;
        #endregion Private Variables
    }
}