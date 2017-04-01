using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;

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
		if (Advertisement.IsReady("rewardedVideo"))
		{
			return true;
		}
        return false;
    }

    public void PlayVideoAd(System.Action <bool> completionAction)
    {
        if (Advertisement.IsReady("rewardedVideo"))
        {
            var options = new ShowOptions { resultCallback = (result)=>
            {
                bool sucsess = false;
                switch (result)
                {
                    case ShowResult.Finished:
                            {
                                Debug.Log("The ad was successfully shown.");
                                sucsess = true;
                                break;
                            }
                    case ShowResult.Skipped:
                            {
                                Debug.Log("The ad was skipped before reaching the end.");
                                break;
                            }
                        case ShowResult.Failed:
                            {
                                Debug.LogError("The ad failed to be shown.");
                                break;
                            }

                    }
					if (completionAction != null)
					{
						completionAction(true);
					}
           		 }
            };
            Advertisement.Show("rewardedVideo", options);
        }
    }

    private void HandleShowResult(ShowResult result)
    {
        switch (result)
        {
            case ShowResult.Finished:
                Debug.Log("The ad was successfully shown.");
                //
                // YOUR CODE TO REWARD THE GAMER
                // Give coins etc.
                break;
            case ShowResult.Skipped:
                Debug.Log("The ad was skipped before reaching the end.");
                break;
            case ShowResult.Failed:
                Debug.LogError("The ad failed to be shown.");
                break;
        }
    }

    #endregion
}
