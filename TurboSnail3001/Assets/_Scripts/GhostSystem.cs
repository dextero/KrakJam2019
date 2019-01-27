using System;
using System.IO;
using TMPro;
using TurboSnail3001;
using UnityEngine;

public class GhostSystem : MonoBehaviour
{
    #region Public Types
    [Serializable]
    public class FrameData
    {
        public long Frame;

        public float Left;
        public float Right;
        public float Velocity;
    }
    #endregion Public Types

    #region Inspector Variables
    [SerializeField] private GameObject _GhostPrefab;
    [SerializeField] private Material _GhostMaterial;
    [SerializeField] private GameObject _NamePlate;
    [SerializeField] private string _GhostLayer;
    [SerializeField] private int _MaxGhosts = 3;
    #endregion Inspector Variables

    #region Unity Methods
    private void Start()
    {
        // prevent collisions between ghosts
        int ghostLayerId = LayerMask.NameToLayer(_GhostLayer);
        Physics.IgnoreLayerCollision(ghostLayerId, ghostLayerId);

        var saves = GameController.Instance.SaveSystem.Load();
        for (int i = 0; i < Math.Min(saves.Saves.Count, _MaxGhosts); ++i)
        {
            CreateGhost(saves.Saves[i]);
        }
    }
    #endregion Unity Methods

    #region Private Methods
    public void CreateGhost(Save save)
    {
        var ghost = Instantiate(_GhostPrefab);
        ghost.name = "Ghost";
        Destroy(ghost.transform.Find("Particles").gameObject);

        var nameplate = Instantiate(_NamePlate);
        nameplate.transform.SetParent(ghost.transform);
        var text = nameplate.GetComponentInChildren<TextMeshProUGUI>();
        text.text = save.Nickname;

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
