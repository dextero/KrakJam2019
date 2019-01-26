using Sirenix.OdinInspector;

namespace TurboSnail3001
{
    using System;
    using UnityEngine;

    public class Snail : MonoBehaviour
    {
        #region Public Variables
        public Transform Steering => _Steering;
        public Transform Drivetrain => _Drivetrain;
        #endregion Public Variables

        #region Inspector Variables
        [SerializeField] private Transform _Steering;
        [SerializeField] private Transform _Drivetrain;
        [SerializeField] private float _DriftThreshold = 10.0f;
        [SerializeField] private KanseiDorifto _KanseiDorifto;
        #endregion Inspector Variables

        #region Unity Methods
        private void Awake()
        {
            _Transform = GetComponent<Transform>();
            _Rigidbody = GetComponent<Rigidbody>();
        }

        private void Update()
        {
            _Angle = Vector3.SignedAngle(_Transform.forward, _Rigidbody.velocity, Vector3.up);
            _KanseiDorifto.enabled = (Mathf.Abs(_Angle) > _DriftThreshold);
        }
        #endregion Unity Methods

        #region Private Variables
        private Transform _Transform;
        private Rigidbody _Rigidbody;

        [ShowInInspector, ReadOnly] private float _Angle;
        #endregion Private Variables
    }
}