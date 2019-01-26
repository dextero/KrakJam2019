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
        public class Controller
        {
            public IController Reference;

            public float Position;
            public float Velocity;
            public float Acceleration;

            [HideInInspector] public float PreviousPosition;
            [HideInInspector] public float PreviousVelocity;
        }
        #endregion Public Types

        #region Public Variables
        public Controller LeftController => _LeftController;
        public Controller RightController => _RightController;
        #endregion Public Variables

        #region Inspector Variables
        [SerializeField, FoldoutGroup("References")]
        private IController _LeftHardware;

        [SerializeField, FoldoutGroup("References")]
        private IController _RightHardware;

        [SerializeField, FoldoutGroup("References")]
        private IController _LeftMockup;

        [SerializeField, FoldoutGroup("References")]
        private IController _RightMockup;

        [SerializeField, FoldoutGroup("Settings")]
        private InputType _Type;
        #endregion Inspector Variables

        #region Unity Methods
        private void Awake()
        {
            /* select controllers */
            switch (_Type)
            {
                case InputType.Hardware:
                {
                    _LeftController.Reference = _LeftHardware;
                    _RightController.Reference = _LeftHardware;

                    _LeftMockup.gameObject.SetActive(false);
                    _RightMockup.gameObject.SetActive(false);
                    break;
                }
                case InputType.Mockup:
                {
                    _LeftController.Reference  = _LeftMockup;
                    _RightController.Reference = _RightMockup;

                    _LeftHardware.gameObject.SetActive(false);
                    _RightHardware.gameObject.SetActive(false);
                    break;
                }
                default:
                throw new ArgumentOutOfRangeException();
            }

            /* enable selected */
            _LeftController.Reference.gameObject.SetActive(true);
            _RightController.Reference.gameObject.SetActive(true);
        }
        private void FixedUpdate()
        {
            UpdateState(_LeftController);
            UpdateState(_RightController);
        }
        #endregion Unity Methods

        #region Private Variables
        [ShowInInspector, ReadOnly, FoldoutGroup("Preview")] private readonly Controller _LeftController  = new Controller();
        [ShowInInspector, ReadOnly, FoldoutGroup("Preview")] private readonly Controller _RightController = new Controller();
        #endregion Private Variables

        #region Private Methods
        private static void UpdateState(Controller controller)
        {
            controller.PreviousPosition = controller.Position;
            controller.PreviousVelocity = controller.Velocity;

            controller.Position = controller.Reference.State;
            controller.Velocity = (controller.PreviousPosition - controller.Position) / Time.deltaTime;
            controller.Acceleration = (controller.PreviousVelocity - controller.Velocity) / Time.deltaTime;
        }
        #endregion Private Methods
    }
}