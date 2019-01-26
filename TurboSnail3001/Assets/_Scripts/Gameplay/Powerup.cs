using System;
using TurboSnail3001;
using UnityEngine;

public class Powerup : MonoBehaviour
{
    #region Public Types
    public enum PowerupType
    {
        None,
        Speeeed,
        Finish
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

        switch (_Type)
        {
            case PowerupType.None:
            {
                break;
            }
            case PowerupType.Speeeed:
            {
                break;
            }
            case PowerupType.Finish:
            {
                GameController.Instance.Finish(FinishResult.Finished);
                break;
            }

            default:
            {
                throw new ArgumentOutOfRangeException();
            }
        }

        _Collected = true;
        Destroy(gameObject);
    }
    #endregion Unity Methods

    #region Private Variables
    private bool _Collected;
    #endregion Private Variables
}