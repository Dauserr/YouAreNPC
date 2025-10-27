using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    [Header("Game Scene")]
    public GameObject gameObjectsRoot;    // Parent GameObject that contains all game objects (NOT cameras)

    [Header("UI Panels")]
    public GameObject mainMenuPanel;      // Main menu with play button
    public GameObject gameHUD;            // In-game HUD with pause button
    public GameObject pausePanel;         // Pause panel with resume button
    public GameObject shopPanel;          // Shop panel with exit button

    [Header("Buttons")]
    public Button playButton;
    public Button pauseButton;             // Pause button in game corner
    public Button resumeButton;            // Resume button in pause panel
    public Button exitButton;              // Exit button in shop panel

    [Header("Game Manager Reference")]
    public GameManager gameManager;

    void Start()
    {
        // Freeze game immediately on start
        Time.timeScale = 0f;

        // Hide all panels first to ensure clean state
        if (gameHUD != null)
            gameHUD.SetActive(false);
        
        if (pausePanel != null)
            pausePanel.SetActive(false);
        
        if (shopPanel != null)
            shopPanel.SetActive(false);

        // Get GameManager if not assigned
        if (gameManager == null)
        {
            gameManager = GameManager.Instance;
        }

        // Setup button listeners
        SetupButtons();
        
        // Show main menu on start
        ShowMainMenu();
    }

    void SetupButtons()
    {
        // Play Button
        if (playButton != null)
        {
            playButton.onClick.AddListener(OnPlayButtonClick);
        }

        // Pause Button (in game corner)
        if (pauseButton != null)
        {
            pauseButton.onClick.AddListener(OnPauseButtonClick);
        }

        // Resume Button (in pause panel)
        if (resumeButton != null)
        {
            resumeButton.onClick.AddListener(OnResumeButtonClick);
        }

        // Exit Button (in shop panel)
        if (exitButton != null)
        {
            exitButton.onClick.AddListener(OnExitButtonClick);
        }
    }

    // Show Main Menu (start of game)
    public void ShowMainMenu()
    {
        // Pause game in menu
        Time.timeScale = 0f;

        // Hide game objects (but keep cameras active)
        if (gameObjectsRoot != null)
            gameObjectsRoot.SetActive(false);

        if (mainMenuPanel != null)
            mainMenuPanel.SetActive(true);
        
        if (gameHUD != null)
            gameHUD.SetActive(false);
        
        if (pausePanel != null)
            pausePanel.SetActive(false);
        
        if (shopPanel != null)
            shopPanel.SetActive(false);
    }

    // Show Game HUD (during gameplay)
    public void ShowGameHUD()
    {
        // Resume game time
        Time.timeScale = 1f;

        // Show game objects
        if (gameObjectsRoot != null)
            gameObjectsRoot.SetActive(true);
        
        if (gameHUD != null)
            gameHUD.SetActive(true);
        
        if (mainMenuPanel != null)
            mainMenuPanel.SetActive(false);
        
        if (pausePanel != null)
            pausePanel.SetActive(false);
        
        if (shopPanel != null)
            shopPanel.SetActive(false);
    }

    // Show Pause Panel (when paused)
    public void ShowPausePanel()
    {
        // Stop game time - game frozen behind
        Time.timeScale = 0f;
        
        // Hide pause button
        if (pauseButton != null)
            pauseButton.gameObject.SetActive(false);
        
        // Show pause panel
        if (pausePanel != null)
            pausePanel.SetActive(true);
        
        // Hide game HUD when pause panel is shown
        if (gameHUD != null)
            gameHUD.SetActive(false);
    }

    // Hide Pause Panel (resume game)
    public void HidePausePanel()
    {
        // Hide pause panel
        if (pausePanel != null)
            pausePanel.SetActive(false);
        
        // Show pause button again
        if (pauseButton != null)
            pauseButton.gameObject.SetActive(true);
        
        // Show game HUD again
        if (gameHUD != null)
            gameHUD.SetActive(true);
        
        // Resume game time
        Time.timeScale = 1f;
    }

    // Show Shop Panel
    public void ShowShopPanel()
    {
        // Stop game time
        Time.timeScale = 0f;
        
        // Hide other panels
        if (mainMenuPanel != null)
            mainMenuPanel.SetActive(false);
        
        if (pausePanel != null)
            pausePanel.SetActive(false);
        
        // Hide game HUD when shop opens
        if (gameHUD != null)
            gameHUD.SetActive(false);
        
        if (shopPanel != null)
            shopPanel.SetActive(true);
    }

    // Hide Shop Panel
    public void HideShopPanel()
    {
        // Hide shop panel
        if (shopPanel != null)
            shopPanel.SetActive(false);
        
        // Show game HUD again when shop closes
        if (gameHUD != null)
            gameHUD.SetActive(true);
        
        // Resume game time
        Time.timeScale = 1f;
    }

    // Button Click Handlers
    public void OnPlayButtonClick()
    {
        if (gameManager != null)
        {
            gameManager.StartGame();
        }
        
        ShowGameHUD();
        Debug.Log("Game Started!");
    }

    public void OnPauseButtonClick()
    {
        ShowPausePanel();
        Debug.Log("Game Paused!");
    }

    public void OnResumeButtonClick()
    {
        HidePausePanel();
        Debug.Log("Game Resumed!");
    }

    public void OnExitButtonClick()
    {
        HideShopPanel();
        Debug.Log("Shop Closed!");
    }

    void Update()
    {
        // Check for ESC key to pause/resume
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (gameManager != null && gameManager.IsGameActive)
            {
                OnPauseButtonClick();
            }
        }
    }
}

