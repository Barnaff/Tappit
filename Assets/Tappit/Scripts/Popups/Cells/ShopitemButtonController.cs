using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopitemButtonController : MonoBehaviour {

    #region Public

    public delegate void ShopitemClickDelegate(ShopItem shopItem);

    public event ShopitemClickDelegate OnShopItemClick;

    #endregion

    #region Private Properties

    [SerializeField]
    private Text _itemLabel;

    [SerializeField]
    private Image _itemIcon;

    [SerializeField]
    private ShopItem _shopItem;

    #endregion


    #region Public

    public void SetShopItem(ShopItem shopItem)
    {
        _shopItem = shopItem;

        _itemLabel.text = _shopItem.ItemName;

        
    }

    public void ClickAction()
    {
        if (OnShopItemClick != null)
        {
            OnShopItemClick(_shopItem);
        }
    }

    #endregion
}
