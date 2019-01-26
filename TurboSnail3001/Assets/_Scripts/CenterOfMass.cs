using Sirenix.OdinInspector;

namespace TurboSnail3001
{
    using UnityEngine;

    public class CenterOfMass : MonoBehaviour
    {
        #region Inspector Variables
        [SerializeField, FoldoutGroup("References")] private Rigidbody _Rigidbody;
        #endregion Inspector Variables

        #region Unity Methods
        private void Awake()
        {
            _Rigidbody.centerOfMass = transform.localPosition;
        }
        #endregion Unity Methods
    }
}