using UnityEngine;

[ExecuteInEditMode]
public class MeshUpdater : MonoBehaviour
{
    [SerializeField] private MeshFilter _Filer;
    [SerializeField] private MeshCollider _Collider;

    private void Update()
    {
        if (_Collider.sharedMesh != _Filer.sharedMesh)
        {
            _Collider.sharedMesh = _Filer.sharedMesh;
        }
    }
}