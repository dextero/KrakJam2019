using System.Collections;

namespace TurboSnail3001.Input
{
    using Sirenix.OdinInspector;
    using UnityEngine;

    public class MockupSystem : MonoBehaviour
    {
        #region Inspector Variables
        [SerializeField, FoldoutGroup("References")]
        private MockupController _Left;

        [SerializeField, FoldoutGroup("References")]
        private MockupController _Right;

        [SerializeField, FoldoutGroup("Settings")]
        private float _Time = 0.5f;

        [SerializeField, FoldoutGroup("Curves")]
        private AnimationCurve _Towards;

        [SerializeField, FoldoutGroup("Curves")]
        private AnimationCurve _Away;
        #endregion Inspector Variables

        #region Unity Methods
        private void Update()
        {
#if UNITY_EDITOR
            UnityEditor.EditorUtility.SetDirty(this);
#endif
        }
        #endregion Unity Methods

        #region Private Variables
        [ShowInInspector, ReadOnly, FoldoutGroup("Preview")] private float _LeftState => _Left != null ? _Left.State : -1.0f;
        [ShowInInspector, ReadOnly, FoldoutGroup("Preview")] private float _RightState => _Right != null ? _Right.State : -1.0f;
        #endregion Private Variables

        #region Private Methods
        [Button, FoldoutGroup("Debug")]
        private void RightPush()
        {
            StartCoroutine(Move(_Right, _Towards, _Time));
        }
        [Button, FoldoutGroup("Debug")]
        private void RightPull()
        {
            StartCoroutine(Move(_Right, _Away, _Time));
        }
        [Button, FoldoutGroup("Debug")]
        private void LeftPush()
        {
            StartCoroutine(Move(_Left, _Towards, _Time));
        }
        [Button, FoldoutGroup("Debug")]
        private void LeftPull()
        {
            StartCoroutine(Move(_Right, _Away, _Time));
        }
        [Button, FoldoutGroup("Debug")]
        private void BothPush()
        {
            StartCoroutine(Move(_Right, _Towards, _Time));
            StartCoroutine(Move(_Left, _Towards, _Time));
        }
        [Button, FoldoutGroup("Debug")]
        private void BothPull()
        {
            StartCoroutine(Move(_Right, _Away, _Time));
            StartCoroutine(Move(_Left, _Away, _Time));
        }

        private IEnumerator Move(MockupController controller, AnimationCurve curve, float length)
        {
            float time = 0.0f;
            while (time < length)
            {
                float s = time / length;
                float value = curve.Evaluate(s);
                controller.State = value;

                time += Time.deltaTime;
                yield return null;
            }

            controller.State = curve.Evaluate(1.0f);
        }
        #endregion Private Methods
    }
}