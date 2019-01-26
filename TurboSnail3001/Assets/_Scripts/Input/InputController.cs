namespace TurboSnail3001.Input
{
    using Sirenix.OdinInspector;
    using UnityEngine;

    public class InputController : MonoBehaviour
    {
        #region Public Types
        public class Controller
        {
            [InlineEditor]
            public HardwareController Reference;

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
        private HardwareController _LeftHardware;

        [SerializeField, FoldoutGroup("References")]
        private HardwareController _RightHardware;
        #endregion Inspector Variables

        #region Unity Methods
        private void Awake()
        {
            _LeftController.Reference = _LeftHardware;
            _RightController.Reference = _RightHardware;

            /* read settings */
            _LeftHardware.MinFrequency = Settings.Instance.LeftControllerSettings.MinFrequency;
            _LeftHardware.MaxFrequency = Settings.Instance.LeftControllerSettings.MaxFrequency;
            _RightHardware.MinFrequency = Settings.Instance.RightControllerSettings.MinFrequency;
            _RightHardware.MaxFrequency = Settings.Instance.RightControllerSettings.MaxFrequency;
        }
        private void FixedUpdate()
        {
            UpdateState(_LeftController);
            UpdateState(_RightController);

#if UNITY_EDITOR
            Sirenix.Utilities.Editor.GUIHelper.RequestRepaint();
#endif
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