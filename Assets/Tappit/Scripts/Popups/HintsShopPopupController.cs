using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class HintsShopPopupController : PopupBaseController {

    #region Private Properties

    [SerializeField]
    private GameObject _watchVideoButton;

    [SerializeField]
    private GameObject _shopItemButton;

    [SerializeField]
    private GameObject _closeButton;

    [SerializeField]
    private Transform _buttonsContent;

    [SerializeField]
    private List<CanvasGroup> _fadeGroups;

    #endregion

    #region Initialization

    // Use this for initialization
    void Start ()
    {
        PopulateShop();
        DisplayIntroAnimation();
	}

    #endregion


    #region Public - User Interactions

    public void WatchVideoButtonAction()
    {
        UseHint();
    }

    public void BuyHintsPackButtonAction()
    {
        UseHint();
    }

    public void CloseButtonAction()
    {
        DisplayCloseAnimation(()=>
        {
            ClosePopup();
        });
    }

    #endregion

    #region Private

    private void UseHint()
    {
        AccountManager.Instance.AddHints(1);
        DisplayCloseAnimation(() =>
        {
            ClosePopup();
        });
    }

    private void PopulateShop()
    {

    }

    private void DisplayIntroAnimation()
    {
        CanvasGroup canvasGroup = this.gameObject.GetComponent<CanvasGroup>();
        canvasGroup.alpha = 0f;
        canvasGroup.DOFade(1f, 0.5f);

        UnityStandardAssets.ImageEffects.Blur blurEffect = Camera.main.GetComponent<UnityStandardAssets.ImageEffects.Blur>();
        if (blurEffect != null)
        {
            blurEffect.enabled = true;
        }

        for (int i = 0; i < _fadeGroups.Count; i++)
        {
            CanvasGroup fadeItem = _fadeGroups[i];
            fadeItem.alpha = 0;
            fadeItem.DOFade(1.0f, 0.5f).SetDelay(0.3f + (i * 0.2f));
        }
    }

    private void DisplayCloseAnimation(System.Action completionAction)
    {
        UnityStandardAssets.ImageEffects.Blur blurEffect = Camera.main.GetComponent<UnityStandardAssets.ImageEffects.Blur>();
        if (blurEffect != null)
        {
            blurEffect.enabled = false;
        }

        CanvasGroup canvasGroup = this.gameObject.GetComponent<CanvasGroup>();
        canvasGroup.DOFade(0f, 0.5f).OnComplete(() =>
        {
            if (completionAction != null)
            {
                completionAction();
            }
        });
    }

    #endregion
}
