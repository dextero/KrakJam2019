namespace TurboSnail3001
{
    using Sirenix.OdinInspector;
    using UnityEngine;

    public class CameraController : MonoBehaviour
    {
        #region Inspector Variables
        [SerializeField, FoldoutGroup("Settings")]
        private float _TranslationSpeed = 1.0f;

        [SerializeField, FoldoutGroup("Settings")]
        private float _RotationSpeed = 1.0f;

        [SerializeField] private float _HeadingSpeed = 1.0f;
        #endregion Inspector Variables

        #region Unity Methods
        private void Awake()
        {
            _Target = GameController.Instance.Target.transform;
            _Transform         = GetComponent<Transform>();

            _TranslationOffset = _Transform.position - _Target.position;
            _RotationOffset = _Transform.localRotation.eulerAngles.x - _Target.localRotation.eulerAngles.x;
        }

        private void FixedUpdate()
        {
            var offset = Quaternion.Euler(_Target.localRotation.eulerAngles.x, 0.0f, 0.0f) * _TranslationOffset;
            offset = Quaternion.Euler(0.0f, _Target.eulerAngles.y, 0.0f) * offset;

            _Transform.position = Vector3.Lerp(_Transform.position, _Target.position + offset,
                Time.deltaTime * _TranslationSpeed);

            _Transform.rotation = Quaternion.Slerp(_Transform.rotation, Quaternion.Euler(_Transform.eulerAngles.x, _Target.eulerAngles.y, _Transform.eulerAngles.z), Time.deltaTime * _RotationSpeed);
            _Transform.localRotation = Quaternion.Slerp(_Transform.localRotation,
                  Quaternion.Euler(_Target.localRotation.eulerAngles.x + _RotationOffset, _Transform.localRotation.eulerAngles.y, _Transform.localRotation.eulerAngles.z),
                Time.deltaTime           * _HeadingSpeed);
        }
        #endregion Unity Methods

        #region Private Variables
        private Transform _Transform;
        private Transform _Target;

        private Vector3    _TranslationOffset;
        private float _RotationOffset;
        #endregion Private Variables
    }
}