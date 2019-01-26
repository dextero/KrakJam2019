using TurboSnail3001;
using UnityEngine;

public class Powerup : MonoBehaviour
{
    #region Public Types
    public enum PowerupType
    {
        None,
        Speeeed,
    }
    #endregion Public Types

    #region Inspector Variables
    [SerializeField] private PowerupType _Type;
    #endregion Inspector Variables

    #region Unity Methods
    private void OnTriggerEnter(Collider collider)
    {
        if (_Collected) { return; }

        var obj = collider.attachedRigidbody.gameObject;
        var player = obj.GetComponent<Snail>();

        _Collected = true;
        Destroy(gameObject);
    }
    #endregion Unity Methods

    #region Private Variables
    private bool _Collected;
    #endregion Private Variables
}