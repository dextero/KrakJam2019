using System;
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
        var ghost = Instantiate(_GhostPrefab).GetComponent<GhostInput>();
        ghost.Initialize(save);
    }
    #endregion Private Methods
}