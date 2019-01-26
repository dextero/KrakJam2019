using System;

namespace TurboSnail3001.Input
{
    using Sirenix.OdinInspector;
    using UnityEngine;

    public class InputController : SerializedMonoBehaviour
    {
        #region Public Types
        public enum InputType
        {
            Hardware,
            Mockup
        }

        public enum State
        {
            Pull,
            Center,
            Push
        }
        #endregion Public Types

        #region Inspector Variables
        [SerializeField, FoldoutGroup("References")]
        private IController _Left;

        [SerializeField, FoldoutGroup("References")]
        private IController _Right;

        [SerializeField, FoldoutGroup("References")]
        private IController _LeftMockup;

        [SerializeField, FoldoutGroup("References")]
        private IController _RightMockup;

        [SerializeField, FoldoutGroup("References")]
        private Snail _Snail;

        [SerializeField, FoldoutGroup("Settings")]
        private InputType _Type;
        #endregion Inspector Variables

        #region Unity Methods
        private void Awake()
        {
            /* enable selected controllers */
            switch (_Type)
            {
                case InputType.Hardware:
                {
                    _LeftMockup.gameObject.SetActive(false);
                    _RightMockup.gameObject.SetActive(false);

                    _Left.gameObject.SetActive(true);
                    _Right.gameObject.SetActive(true);
                    break;
                }
                case InputType.Mockup:
                {
                    _Left.gameObject.SetActive(false);
                    _Right.gameObject.SetActive(false);

                    _LeftMockup.gameObject.SetActive(true);
                    _RightMockup.gameObject.SetActive(true);
                    break;
                }
                default:
                throw new ArgumentOutOfRangeException();
            }
        }
        private void FixedUpdate()
        {
            UpdateState(_LeftController, ref _LeftState);
            UpdateState(_RightController, ref _RightState);

            _Snail.Left(_LeftController.State);
        }
        #endregion Unity Methods

        #region Private Variables
        private IController _LeftController => _Type == InputType.Hardware ? _Left : _LeftMockup;
        private IController _RightController => _Type == InputType.Hardware ? _Right : _RightMockup;

        [ShowInInspector, FoldoutGroup("Preview")]
        private float _LeftValue => _LeftController?.State ?? -1.0f;

        [ShowInInspector, FoldoutGroup("Preview")]
        private float _RightValue => _RightController?.State ?? -1.0f;

        [ShowInInspector, FoldoutGroup("Preview")]
        private State _LeftState = State.Center;
        private State _RightState = State.Center;
        #endregion Private Variables

        #region Private Methods
        private void UpdateState(IController controller, ref State state)
        {
            var value = controller.State;
            switch (state)
            {
                case State.Pull:
                {
                    if (value < 0.7f) { state = State.Center; Debug.Log(controller.name + ": center"); }
                    return;
                }
                case State.Center:
                {
                    if (value < 0.2f) { state = State.Push; Debug.Log(controller.name + ": push"); }
                    if (value > 0.8f) { state = State.Pull; Debug.Log(controller.name + ": pull"); }

                    return;
                }
                case State.Push:
                {
                    if (value > 0.3f) { state = State.Center; Debug.Log(controller.name + ": center"); };
                    return;
                }
            }
        }
        #endregion Private Methods
    }
}