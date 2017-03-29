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
        _watchVideoButton.SetActive(false);
        _shopItemButton.SetActive(false);
        _closeButton.SetActive(false);
        PopulateShop();
        DisplayIntroAnimation();
	}

    #endregion


    #region Public - User Interactions

    public void WatchVideoButtonAction()
    {
        AdsManager.Instance.PlayVideoAd((sucsess) =>
        {
            AddHints(1);
        });
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


    private void AddHints(int amount)
    {
        AccountManager.Instance.AddHints(amount);
        DisplayCloseAnimation(() =>
        {
            ClosePopup();
        });
    }

    private void PopulateShop()
    {
        if (AdsManager.Instance.CanWatchVideoAd())
        {
            GameObject watchVideoButton = Instantiate(_watchVideoButton);
            watchVideoButton.transform.SetParent(_buttonsContent);
            watchVideoButton.transform.localScale = Vector3.one;
            watchVideoButton.SetActive(true);
            _fadeGroups.Add(watchVideoButton.GetComponent<CanvasGroup>());
        }

        List<ShopItem> shopItems = ShopManager.Instance.GetShopItems();

        for (int i = 0; i < shopItems.Count; i++)
        {  
            GameObject shopItemButton = Instantiate(_shopItemButton);
            shopItemButton.transform.SetParent(_buttonsContent);
            shopItemButton.transform.localScale = Vector3.one;
            shopItemButton.SetActive(true);
            _fadeGroups.Add(shopItemButton.GetComponent<CanvasGroup>());

            ShopitemButtonController shopItemButtonController = shopItemButton.GetComponent<ShopitemButtonController>();
            if (shopItemButtonController != null)
            {
                shopItemButtonController.SetShopItem(shopItems[i]);
                shopItemButtonController.OnShopItemClick += OnShopItemClickHandler;
            }
        }

        GameObject closeButton = Instantiate(_closeButton);
        closeButton.transform.SetParent(_buttonsContent);
        closeButton.transform.localScale = Vector3.one;
        closeButton.SetActive(true);
        _fadeGroups.Add(closeButton.GetComponent<CanvasGroup>());
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


    #region Events

    private void OnShopItemClickHandler(ShopItem shopItem)
    {
        ShopManager.Instance.BuyShopItem(shopItem, (sucsess)=>
        {
            if (sucsess)
            {
                AddHints(shopItem.Amount);
            }
        });
    }

    #endregion
}

