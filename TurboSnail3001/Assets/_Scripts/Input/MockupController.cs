namespace TurboSnail3001.Input
{
    using Sirenix.OdinInspector;
    using UnityEngine;

    public class MockupController : MonoBehaviour, IController
    {
        #region Public Variables
        public float State
        {
            get => 1.0f - _State;
            set => _State = value;
        }
        #endregion Public Variables

        #region Private Variables
        [SerializeField, FoldoutGroup("Preview")]
        private float _State;
        #endregion Private Variables
    }
}