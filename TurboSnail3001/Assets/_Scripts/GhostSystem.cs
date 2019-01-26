using System;
using TurboSnail3001;
using UnityEngine;

public class GhostSystem : MonoBehaviour
{
    #region Public Types
    [Serializable]
    public class FrameData
    {
        public long  Frame;

        public float Left;
        public float Right;
        public float Velocity;
    }
    #endregion Public Types

    #region Inspector Variables
    [SerializeField] private GameObject _GhostPrefab;
    [SerializeField] private Material _GhostMaterial;
    [SerializeField] private string _GhostLayer;
    #endregion Inspector Variables

    #region Unity Methods
    private void Start()
    {
        var saves = GameController.Instance.SaveSystem.Load();
        if (saves.Saves.Count != 0)
        {
            CreateGhost(saves.Saves[0]);
        }
    }
    #endregion Unity Methods

    #region Private Methods
    public void CreateGhost(Save save)
    {
        var ghost = Instantiate(_GhostPrefab);
        ghost.name = "Ghost";

        /* remove all player scripts */
        var snailInput = ghost.GetComponent<SnailInput>();
        Destroy(snailInput);

        /* change material */
        var renderer = ghost.GetComponentInChildren<Renderer>();
        renderer.material = _GhostMaterial;

        /* change layer */
        ghost.layer = LayerMask.NameToLayer(_GhostLayer);
        var children = ghost.GetComponentsInChildren<Transform>(true);
        foreach (var child in children) { child.gameObject.layer = LayerMask.NameToLayer(_GhostLayer); }

        /* add ghost scripts */
        var ghostInput = ghost.AddComponent<GhostInput>();
        ghostInput.Initialize(save);
    }
    #endregion Private Methods
}