using UnityEngine;

public class SphereGizmo : MonoBehaviour
{
    #region Inspector Variables
    [SerializeField] private Color _Color = Color.white;
    [SerializeField] private float _Radius = 0.1f;
    #endregion Inspector Variables

    #region Unity Methods
    private void OnDrawGizmos()
    {
        Gizmos.color = _Color;
        Gizmos.DrawWireSphere(transform.position, _Radius);
    }
    #endregion Unity Methods
}