using UnityEngine;
using IDosGames;

public class ShopTrigger : MonoBehaviour
{
    [Header("Shop Settings")]
    public float triggerRadius = 5f; // Distance to open shop
    public GameObject shopPanel; // Shop panel to open
    public bool freezeGameWhenOpen = true;
    
    [Header("References")]
    public Transform player; // Player reference
    public UIManager uiManager; // Reference to UIManager
    
    private bool shopIsOpen = false;
    private ShopWindow shopWindow; // Shop window reference
    
    void Start()
    {
        // Find player if not assigned
        if (player == null)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null)
            {
                player = playerObj.transform;
            }
        }
        
        // Try to find ShopWindow component
        if (shopWindow == null)
        {
            shopWindow = FindObjectOfType<ShopWindow>();
        }
        
        // Try to find UIManager if not assigned
        if (uiManager == null)
        {
            uiManager = FindObjectOfType<UIManager>();
        }
    }
    
    void Update()
    {
        if (player == null) return;
        
        float distance = Vector3.Distance(transform.position, player.position);
        
        // Only open once when player enters radius
        if (distance <= triggerRadius && !shopIsOpen)
        {
            OpenShop();
        }
        // Close when player leaves radius
        else if (distance > triggerRadius && shopIsOpen)
        {
            CloseShop();
        }
    }
    
    void OpenShop()
    {
        if (shopIsOpen) return; // Already open, don't reopen
        
        shopIsOpen = true;
        
        // Use UIManager to open shop (this handles HUD hiding and time scaling)
        if (uiManager != null)
        {
            uiManager.ShowShopPanel();
            Debug.Log("Shop opened via UIManager - Player entered radius!");
        }
        // Fallback: Try to open shop using IDosGamesSDK system directly
        else if (shopWindow != null)
        {
            shopWindow.gameObject.SetActive(true);
            if (freezeGameWhenOpen)
            {
                Time.timeScale = 0f;
            }
            Debug.Log("Shop opened via ShopWindow - Player entered radius!");
        }
        else if (shopPanel != null)
        {
            shopPanel.SetActive(true);
            if (freezeGameWhenOpen)
            {
                Time.timeScale = 0f;
            }
            Debug.Log("Shop opened via shopPanel GameObject!");
        }
        else
        {
            Debug.LogWarning("No shop window or panel found! Assign Shop Panel in Inspector.");
        }
    }
    
    void CloseShop()
    {
        if (!shopIsOpen) return; // Already closed, don't close again
        
        shopIsOpen = false;
        
        // Use UIManager to close shop (this handles HUD showing and time scaling)
        if (uiManager != null)
        {
            uiManager.HideShopPanel();
            Debug.Log("Shop closed - Player left radius! Can reopen by entering again.");
        }
        // Fallback: Close shop using IDosGamesSDK system directly
        else if (shopWindow != null)
        {
            shopWindow.gameObject.SetActive(false);
            if (freezeGameWhenOpen)
            {
                Time.timeScale = 1f;
            }
        }
        else if (shopPanel != null)
        {
            shopPanel.SetActive(false);
            if (freezeGameWhenOpen)
            {
                Time.timeScale = 1f;
            }
        }
    }
    
    void OnDrawGizmos()
    {
        // Draw trigger radius in scene view
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, triggerRadius);
    }
    
    void OnDestroy()
    {
        // Resume game when shop trigger is destroyed
        if (shopIsOpen)
        {
            Time.timeScale = 1f;
        }
    }
}

