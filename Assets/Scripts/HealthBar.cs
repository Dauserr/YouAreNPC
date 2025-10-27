using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [Header("Health Bar Settings")]
    public HealthSystem healthSystem;
    public GameObject healthBarPrefab; // Can be null - will create automatically
    public Vector3 offset = new Vector3(0, 1.5f, 0); // Position above NPC
    public bool followParent = true; // Health bar follows NPC
    
    [Header("Health Bar Setup")]
    public float width = 2f;
    public float height = 0.3f;
    public Color fillColor = Color.green;
    public Color backgroundColor = Color.red;
    
    private GameObject healthBarObject;
    private Image healthBarFill;
    private Image healthBarBackground;

    void Start()
    {
        // Get or find HealthSystem component (required!)
        if (healthSystem == null)
        {
            healthSystem = GetComponent<HealthSystem>();
        }

        if (healthSystem == null)
        {
            Debug.LogError($"HealthBar requires a HealthSystem component on {gameObject.name}!");
            return;
        }

        // Create or use existing health bar
        if (healthBarPrefab == null)
        {
            CreateHealthBar();
        }
        else
        {
            healthBarObject = Instantiate(healthBarPrefab, transform.position + offset, Quaternion.identity);
        }

        // Setup health bar visuals
        SetupHealthBar();
        
        // Subscribe to health changes
        healthSystem.OnHealthChanged += UpdateHealthBar;
        
        // Initial update
        UpdateHealthBar(healthSystem.GetHealth());

        // Update position
        UpdateHealthBarPosition();
    }

    void CreateHealthBar()
    {
        // Create canvas for health bar (world space)
        GameObject canvasObj = new GameObject("HealthBarCanvas");
        Canvas canvas = canvasObj.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.WorldSpace;
        canvas.sortingOrder = 100;
        
        CanvasScaler scaler = canvasObj.AddComponent<CanvasScaler>();
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ConstantPixelSize;
        scaler.scaleFactor = 1;
        
        GraphicRaycaster raycaster = canvasObj.AddComponent<GraphicRaycaster>();
        
        // Set canvas size
        RectTransform canvasRect = canvasObj.GetComponent<RectTransform>();
        canvasRect.sizeDelta = new Vector2(width, height);

        // Add to offset position
        canvasObj.transform.SetParent(transform);
        canvasObj.transform.localPosition = offset;
        canvasObj.transform.localRotation = Quaternion.identity;
        canvasObj.transform.localScale = Vector3.one;

        healthBarObject = canvasObj;
    }

    void SetupHealthBar()
    {
        if (healthBarObject == null) return;

        // Create background
        GameObject bgObj = new GameObject("Background");
        bgObj.transform.SetParent(healthBarObject.transform);
        RectTransform bgRect = bgObj.AddComponent<RectTransform>();
        bgRect.anchorMin = new Vector2(0, 0);
        bgRect.anchorMax = new Vector2(1, 1);
        bgRect.sizeDelta = Vector2.zero;
        bgRect.anchoredPosition = Vector2.zero;
        
        healthBarBackground = bgObj.AddComponent<Image>();
        healthBarBackground.color = backgroundColor;

        // Create fill
        GameObject fillObj = new GameObject("Fill");
        fillObj.transform.SetParent(bgObj.transform);
        RectTransform fillRect = fillObj.AddComponent<RectTransform>();
        fillRect.anchorMin = new Vector2(0, 0);
        fillRect.anchorMax = new Vector2(1, 1);
        fillRect.sizeDelta = Vector2.zero;
        fillRect.anchoredPosition = Vector2.zero;
        
        healthBarFill = fillObj.AddComponent<Image>();
        healthBarFill.color = fillColor;
        healthBarFill.type = Image.Type.Filled;
        healthBarFill.fillMethod = Image.FillMethod.Horizontal;
    }

    void Update()
    {
        if (followParent && healthBarObject != null)
        {
            UpdateHealthBarPosition();
        }
    }

    void UpdateHealthBarPosition()
    {
        if (healthBarObject == null) return;

        if (followParent)
        {
            healthBarObject.transform.position = transform.position + offset;
            // Make health bar always face camera
            healthBarObject.transform.LookAt(Camera.main.transform.position);
            healthBarObject.transform.Rotate(0, 180, 0);
        }
    }

    void UpdateHealthBar(int currentHealth)
    {
        if (healthBarFill == null || healthSystem == null) return;

        float healthPercentage = healthSystem.GetHealthPercentage();
        healthBarFill.fillAmount = healthPercentage;
        
        // Optional: Change color based on health
        if (healthPercentage > 0.6f)
        {
            healthBarFill.color = Color.green;
        }
        else if (healthPercentage > 0.3f)
        {
            healthBarFill.color = Color.yellow;
        }
        else
        {
            healthBarFill.color = Color.red;
        }
    }

    void OnDestroy()
    {
        if (healthSystem != null)
        {
            healthSystem.OnHealthChanged -= UpdateHealthBar;
        }
        
        if (healthBarObject != null)
        {
            Destroy(healthBarObject);
        }
    }

    // Public methods to customize health bar
    public void SetOffset(Vector3 newOffset)
    {
        offset = newOffset;
    }

    public void SetHealthBarWidth(float newWidth)
    {
        width = newWidth;
        if (healthBarObject != null)
        {
            RectTransform canvasRect = healthBarObject.GetComponent<RectTransform>();
            canvasRect.sizeDelta = new Vector2(newWidth, height);
        }
    }

    public void SetHealthBarHeight(float newHeight)
    {
        height = newHeight;
        if (healthBarObject != null)
        {
            RectTransform canvasRect = healthBarObject.GetComponent<RectTransform>();
            canvasRect.sizeDelta = new Vector2(width, newHeight);
        }
    }
}

