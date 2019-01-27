using System.Collections.Generic;
using Sirenix.OdinInspector;

namespace TurboSnail3001
{
    using UnityEngine;

    public class Snail : MonoBehaviour
    {
        #region Public Variables
        public Save Save;

        public Transform Steering => _Steering;
        public Transform Drivetrain => _Drivetrain;
        public float Speed { get { return GetComponent<Rigidbody>().velocity.magnitude; }}
        #endregion Public Variables

        #region Inspector Variables
        [SerializeField] private Transform _Steering;
        [SerializeField] private Transform _Drivetrain;
        [SerializeField] private List<ParticleSystem> _Burst;
        #endregion Inspector Variables

        #region Unity Methods
        private void Awake()
        {
            _Transform = GetComponent<Transform>();
            _Rigidbody = GetComponent<Rigidbody>();

            Save.StartPosition = transform.position;
            Save.StartRotation = transform.rotation;
        }

        private void Update()
        {
            _Angle = Vector3.SignedAngle(_Transform.forward, _Rigidbody.velocity, Vector3.up);

            if(Mathf.Abs(_Angle) < 5) { _Lock = false; }
            if(Mathf.Abs(_Angle) > 30 && !_Lock)
            {
                foreach(var burst in _Burst)
                {
                    if(burst == null) { continue; }
                    burst.Play();
                }
                _Lock = true;
            }
        }
        #endregion Unity Methods

        #region Private Variables
        private Transform _Transform;
        private Rigidbody _Rigidbody;
        private bool _Lock = false;


        [ShowInInspector, ReadOnly] private float _Angle;
        #endregion Private Variables

        #region Private Methods
        [Button]
        private void SaveSnail()
        {
            GameController.Instance.SaveSystem.Add(Save);
            GameController.Instance.SaveSystem.Save();
        }
        #endregion Private Methods
    }
}