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

        [SerializeField, FoldoutGroup("Settings")]
        private InputType _Type;
        #endregion Inspector Variables

        #region Unity Methods
        private void FixedUpdate() { }
        #endregion Unity Methods

        #region Private Variables
        private IController _LeftController => _Type == InputType.Hardware ? _Left : _LeftMockup;
        private IController _RightController => _Type == InputType.Hardware ? _Right : _RightMockup;

        [ShowInInspector, FoldoutGroup("Preview")]
        private float _LeftState => _LeftController?.State ?? -1.0f;

        [ShowInInspector, FoldoutGroup("Preview")]
        private float _RightState => _RightController?.State ?? -1.0f;
        #endregion Private Variables
    }
}