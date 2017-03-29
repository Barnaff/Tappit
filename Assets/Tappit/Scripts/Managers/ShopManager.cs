using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopManager : Kobapps.Singleton<ShopManager> {

    #region Private Properties

    private List<ShopItem> _shopItems;

    #endregion


    #region Public

    public void Init()
    {
        LoadShopItems();
    }

    public List<ShopItem> GetShopItems()
    {
        return _shopItems;
    }

    public void BuyShopItem(ShopItem shopItem, System.Action <bool> completionAction)
    {
        if (completionAction != null)
        {
            completionAction(true);
        }
    }

    #endregion


    #region Private

    private void LoadShopItems()
    {
        _shopItems = ShopSettings.Instance.ShopItems;
    }

    #endregion
}
