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
        #endregion Public Variables

        #region Inspector Variables
        [SerializeField] private Transform _Steering;
        [SerializeField] private Transform _Drivetrain;
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
        }
        #endregion Unity Methods

        #region Private Variables
        private Transform _Transform;
        private Rigidbody _Rigidbody;

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