using Sirenix.OdinInspector;

namespace TurboSnail3001
{
    using TurboSnail3001.Input;
    using UnityEngine;

    [RequireComponent(typeof(Snail))]
    public class SnailInput : MonoBehaviour
    {
        #region Inspector Variables
        [SerializeField, FoldoutGroup("References")] private InputController _Input;
        [SerializeField, FoldoutGroup("Settings")] private float _Speed;
        [SerializeField, FoldoutGroup("Settings")] private float _RotateSpeed;
        #endregion Inspector Variables

        #region Unity Methods
        private void Awake()
        {
            _Transform = GetComponent<Transform>();
            _Rigidbody = GetComponent<Rigidbody>();
        }

        private void FixedUpdate()
        {
            /* todo: for debug only */
            if (UnityEngine.Input.GetKey(KeyCode.LeftArrow))
            {
                _Rigidbody.AddTorque(0.0f, -_RotateSpeed * 0.25f, 0.0f);
            }

            if (UnityEngine.Input.GetKey(KeyCode.RightArrow))
            {
                _Rigidbody.AddTorque(0.0f, _RotateSpeed * 0.25f, 0.0f);
            }

            /* get input */
            float left  = _Input.LeftController.Position;
            float right = _Input.RightController.Position;

            /* calculate velocity */
            float velocity = (left +  right);

            _Rigidbody.AddForce(new Vector3(_Transform.forward.x, 0.0f, _Transform.forward.z) * _Speed * velocity);

            _Rigidbody.AddTorque(0.0f, -_RotateSpeed * left, 0.0f);
            _Rigidbody.AddTorque(0.0f, _RotateSpeed * right, 0.0f);
        }
        #endregion Unity Methods

        #region Private Variables
        private Rigidbody _Rigidbody;
        private Transform _Transform;
        #endregion Private Variables
    }
}