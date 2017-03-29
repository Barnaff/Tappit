using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoaderController : MonoBehaviour {

	// Use this for initialization
	void Start ()
    {
        StartCoroutine(LoadingSequance());
	}


    #region Private

    private IEnumerator LoadingSequance()
    {
        yield return 0f;

        AccountManager.Instance.Init();

        yield return 0f;

        ShopManager.Instance.Init();

        yield return 0f;

        AdsManager.Instance.Init();

        yield return 0f;

        FlowManager.Instance.FirstScreen();
    }

    #endregion
}
