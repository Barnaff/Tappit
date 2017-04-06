using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class LevelFailedPopupController : PopupBaseController {


    #region Private Properties

    [SerializeField]
    private CanvasGroup[] _fadeGroups;

    #endregion


    #region Initialization

    void Start()
    {
        DisplayIntroAnimation();
    }

    #endregion


    #region Private

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

        for (int i = 0; i < _fadeGroups.Length; i++)
        {
            CanvasGroup fadeItem = _fadeGroups[i];
            fadeItem.alpha = 0;
            fadeItem.DOFade(1.0f, 0.7f).SetDelay(1.0f + (i * 0.2f));
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


    #region User Interactions


    public void MenuButtonAction()
	{
        FlowManager.Instance.LevelsSelectionScreen();
	}

	public void PlayAgainButtonAction()
	{
        ClosePopup(() =>
        {
            FlowManager.Instance.StartLevel(GameSetupManager.Instance.SelectedLevel);
        });
    }

    #endregion


    #region PopupBaseController Subclassing

    protected override void DisplayPopupCloseAnimation(System.Action completionAction)
    {
        DisplayCloseAnimation(() =>
        {
            if (completionAction != null)
            {
                completionAction();
            }
        });
    }

    #endregion

}
