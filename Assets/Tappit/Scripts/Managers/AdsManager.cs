using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdsManager : Kobapps.Singleton<AdsManager> {

    #region Private Properties

    [SerializeField]
    private System.DateTime _lastVideoAdTime;

    #endregion

    #region Public

    public void Init()
    {

    }

    public bool CanWatchVideoAd()
    {
        return true;
    }

    public void PlayVideoAd(System.Action <bool> completionAction)
    {
        if (completionAction != null)
        {
            completionAction(true);
        }
    }

    #endregion
}
