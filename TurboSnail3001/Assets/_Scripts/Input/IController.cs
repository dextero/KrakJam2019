using UnityEngine;

namespace TurboSnail3001.Input
{
    public interface IController
    {
        #region Variables
        float State { get; }

        GameObject gameObject { get; }
        string name { get; }
        #endregion Variables
    }
}