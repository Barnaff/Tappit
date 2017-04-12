using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Purchasing;

public class IAPManager : Kobapps.Singleton<IAPManager>, IStoreListener
{
    #region Public Properties

    public delegate void PurchaseCompletionDelegate(bool sucsess);

    #endregion


    #region Private Properties

    private IStoreController _storeController;          // The Unity Purchasing system.

    private IExtensionProvider _storeExtensionProvider; // The store-specific Purchasing subsystems.

    private bool _isInitialized;

    #endregion


    #region Public

    public void InitIAP(List<IAPProduct> products)
    {
        if (IsInitialized())
        {
            return;
        }

		Debug.Log("Iniitalizing IAP");

        ConfigurationBuilder builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());

        foreach (IAPProduct product in products)
        {
			Debug.Log("add product: " + product.Identifier + " >> " + product.IOSIdentifier);
            builder.AddProduct(product.Identifier, product.ProductType, new IDs(){
                { product.IOSIdentifier, AppleAppStore.Name },
                { product.AndroidIdntifier, GooglePlay.Name },
            });
        }

        UnityPurchasing.Initialize(this, builder);
    }

    public void BuyProduct(string productIdentifier, PurchaseCompletionDelegate purchaseCompletionAction)
    {
        // If Purchasing has been initialized ...
        if (IsInitialized())
        {
            // ... look up the Product reference with the general product identifier and the Purchasing 
            // system's products collection.
            Product product = _storeController.products.WithID(productIdentifier);

            // If the look up found a product for this device's store and that product is ready to be sold ... 
            if (product != null && product.availableToPurchase)
            {
                Debug.Log(string.Format("Purchasing product asychronously: '{0}'", product.definition.id));
                // ... buy the product. Expect a response either through ProcessPurchase or OnPurchaseFailed 
                // asynchronously.
                _storeController.InitiatePurchase(product);
            }
            // Otherwise ...
            else
            {
                // ... report the product look-up failure situation  
                Debug.Log("BuyProductID: FAIL. Not purchasing product, either is not found or is not available for purchase");
            }
        }
        // Otherwise ...
        else
        {
            // ... report the fact Purchasing has not succeeded initializing yet. Consider waiting longer or 
            // retrying initiailization.
            Debug.Log("BuyProductID FAIL. Not initialized.");
        }
    }

    public string GetProductPrice(string productIdentifier)
    {
        Product product = _storeController.products.WithID(productIdentifier);
        if (product != null)
        {
            return product.metadata.localizedPriceString;
        }
        return "";
    }

    #endregion


    #region Private

    private bool IsInitialized()
    {
        // Only say we are initialized if both the Purchasing references are set.
        return _storeController != null && _storeExtensionProvider != null;
    }

    #endregion


    #region IStoreListener Implementation

    void IStoreListener.OnInitialized(IStoreController controller, IExtensionProvider extensions)
    {
        // Purchasing has succeeded initializing. Collect our Purchasing references.
        Debug.Log("OnInitialized: PASS");

        // Overall Purchasing system, configured with products for this application.
        _storeController = controller;

		foreach (Product product in _storeController.products.all)
		{
			Debug.Log("Got product: " + product.definition.id + " data: "  + product.metadata.ToString());
		}

        // Store specific subsystem, for accessing device-specific store features.
        _storeExtensionProvider = extensions;

    }

    void IStoreListener.OnInitializeFailed(InitializationFailureReason error)
    {
        // Purchasing set-up has not succeeded. Check error for reason. Consider sharing this reason with the user.
        Debug.Log("OnInitializeFailed InitializationFailureReason:" + error);
    }

    void IStoreListener.OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
    {
        // A product purchase attempt did not succeed. Check failureReason for more detail. Consider sharing 
        // this reason with the user to guide their troubleshooting actions.
        Debug.Log(string.Format("OnPurchaseFailed: FAIL. Product: '{0}', PurchaseFailureReason: {1}", product.definition.storeSpecificId, failureReason));
    }

    PurchaseProcessingResult IStoreListener.ProcessPurchase(PurchaseEventArgs purchaseEvent)
    {
        if (purchaseEvent.purchasedProduct != null)
        {
            string productId = purchaseEvent.purchasedProduct.definition.id;

            Debug.Log("ProcessPurchase: SUCSESS. product: " + productId + ".");

            // Return a flag indicating whether this product has completely been received, or if the application needs 
            // to be reminded of this purchase at next app launch. Use PurchaseProcessingResult.Pending when still 
            // saving purchased products to the cloud, and when that save is delayed. 
           
        }
        return PurchaseProcessingResult.Complete;
    }

    #endregion
}

[System.Serializable]
public class IAPProduct
{
    public ProductType ProductType;

    public string Identifier;

    public string IOSIdentifier;

    public string AndroidIdntifier;

}