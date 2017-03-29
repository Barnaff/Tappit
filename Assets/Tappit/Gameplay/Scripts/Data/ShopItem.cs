using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ShopItem  {

    public string ItemIdentifier;

    public string ItemName;

    public eShopItemType ItemType;

    public int Amount;

    public string PriceString;

    public eShopItemPaymentMethod PaymentMethod;

    public string IOSIdentifier;

    public string GooglePlayIdentifier;
}
