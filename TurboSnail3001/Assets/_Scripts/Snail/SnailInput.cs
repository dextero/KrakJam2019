﻿using Sirenix.OdinInspector;

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
            _Snail = GetComponent<Snail>();

            _Transform = GetComponent<Transform>();
            _Rigidbody = GetComponent<Rigidbody>();
        }

        private void FixedUpdate()
        {
            /* todo: for debug only */
            if (UnityEngine.Input.GetKey(KeyCode.LeftArrow))
            {
                _Rigidbody.AddForceAtPosition(-_Transform.right * _RotateSpeed, _Snail.Steering.position);
            }

            if (UnityEngine.Input.GetKey(KeyCode.RightArrow))
            {
                _Rigidbody.AddForceAtPosition(_Transform.right * _RotateSpeed, _Snail.Steering.position);
            }

            /* get input */
            float left  = _Input.LeftController.Position;
            float right = _Input.RightController.Position;

            /* calculate velocity */
            float velocity = (left +  right);

            _Rigidbody.AddForceAtPosition(_Transform.forward * _Speed * velocity, _Snail.Drivetrain.position);

            _Rigidbody.AddForceAtPosition(-_Transform.right * _RotateSpeed * left, _Snail.Steering.position);
            _Rigidbody.AddForceAtPosition(_Transform.right * _RotateSpeed * right, _Snail.Steering.position);
        }
        #endregion Unity Methods

        #region Private Variables
        private Snail _Snail;
        private Rigidbody _Rigidbody;
        private Transform _Transform;
        #endregion Private Variables
    }
}