using UnityEngine;
using IDosGames;

/// <summary>
/// Debug script to print available shop item IDs
/// Press 'P' to print all shop items to console
/// </summary>
public class ShopDebugger : MonoBehaviour
{
    void Update()
    {
        // Press P to print shop items
        if (Input.GetKeyDown(KeyCode.P))
        {
            PrintShopItems();
        }
    }

    void PrintShopItems()
    {
        Debug.Log("=== SHOP ITEMS DEBUG ===");
        
        // Print Real Money products
        if (ShopSystem.ProductsForRealMoney != null)
        {
            Debug.Log($"\n--- Real Money Products ({ShopSystem.ProductsForRealMoney.Count}) ---");
            foreach (var product in ShopSystem.ProductsForRealMoney)
            {
                var itemID = product["ItemID"]?.ToString();
                var itemClass = product["ItemClass"]?.ToString();
                var name = product["Name"]?.ToString();
                var priceRM = product["PriceRM"]?.ToString();
                
                Debug.Log($"ID: {itemID} | Class: {itemClass} | Name: {name} | Price: ${priceRM}");
            }
        }
        
        // Print Virtual Currency products
        if (ShopSystem.ProductsForVirtualCurrency != null)
        {
            Debug.Log($"\n--- Virtual Currency Products ({ShopSystem.ProductsForVirtualCurrency.Count}) ---");
            foreach (var product in ShopSystem.ProductsForVirtualCurrency)
            {
                var itemID = product["ItemID"]?.ToString();
                var itemClass = product["ItemClass"]?.ToString();
                var name = product["Name"]?.ToString();
                var priceVC = product["PriceVC"]?.ToString();
                
                Debug.Log($"ID: {itemID} | Class: {itemClass} | Name: {name} | PriceVC: {priceVC}");
            }
        }
        
        Debug.Log("Press 'P' again to refresh list");
    }

    // You can also call this from a button or inspector
    [ContextMenu("Print Shop Items")]
    void PrintShopItemsContext()
    {
        PrintShopItems();
    }
}

