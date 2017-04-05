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

        List<IAPProduct> products = new List<IAPProduct>();
        foreach (ShopItem shopItem in _shopItems)
        {
            IAPProduct product = new IAPProduct();
            product.Identifier = shopItem.ItemIdentifier;
            product.IOSIdentifier = shopItem.IOSIdentifier;
            product.AndroidIdntifier = shopItem.GooglePlayIdentifier;
            product.ProductType = UnityEngine.Purchasing.ProductType.Consumable;
            products.Add(product);
        }
        IAPManager.Instance.InitIAP(products);
    }

    #endregion
}
