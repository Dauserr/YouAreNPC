using UnityEngine;
using UnityEngine.UI;
using IDosGames;

/// <summary>
/// Helper script for shop item buttons to call BuyForRealMoney or BuyForVirtualCurrency
/// </summary>
public class ShopItemButton : MonoBehaviour
{
    [Header("Purchase Settings")]
    [Tooltip("The item ID to purchase (from your IDosGames dashboard)")]
    public string itemID;
    
    [Tooltip("Purchase type: BuyForRealMoney or BuyForVirtualCurrency")]
    public PurchaseType purchaseType = PurchaseType.RealMoney;
    
    [Tooltip("Currency type (only for VirtualCurrency purchases)")]
    public VirtualCurrencyID currencyID = VirtualCurrencyID.IG;
    
    [Tooltip("Price in virtual currency (only for VirtualCurrency purchases - RealMoney gets price from server)")]
    public float price = 0f;
    
    [Header("References")]
    private Button button;
    
    void Start()
    {
        button = GetComponent<Button>();
        
        if (button == null)
        {
            Debug.LogError("ShopItemButton requires a Button component!");
            return;
        }
        
        // Add onClick listener
        button.onClick.AddListener(OnButtonClick);
    }
    
    void OnButtonClick()
    {
        if (string.IsNullOrEmpty(itemID))
        {
            Debug.LogWarning("ShopItemButton: Item ID is not set!");
            return;
        }
        
        switch (purchaseType)
        {
            case PurchaseType.RealMoney:
                // For RealMoney: Pass only itemID, price comes from server
                ShopSystem.BuyForRealMoney(itemID);
                Debug.Log($"Purchase request for item: {itemID} (Real Money - price comes from server)");
                break;
                
            case PurchaseType.VirtualCurrency:
                // For VirtualCurrency: Pass itemID, currency type, and price
                if (price <= 0)
                {
                    Debug.LogWarning("ShopItemButton: Price must be greater than 0 for VirtualCurrency purchases!");
                    return;
                }
                ShopSystem.BuyForVirtualCurrency(itemID, currencyID, price);
                Debug.Log($"Purchase request for item: {itemID} (Virtual Currency: {currencyID}, Price: {price})");
                break;
        }
    }
    
    void OnDestroy()
    {
        if (button != null)
        {
            button.onClick.RemoveListener(OnButtonClick);
        }
    }
}

public enum PurchaseType
{
    RealMoney,
    VirtualCurrency
}

