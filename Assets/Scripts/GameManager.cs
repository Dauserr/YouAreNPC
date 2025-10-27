using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Singleton instance
    public static GameManager Instance { get; private set; }

    // Game state
    public enum GameState
    {
        Menu,       // Main menu
        Playing,    // Game is running
        Paused,     // Game is paused
        GameOver,   // Player lost
        Win         // Player won
    }

    private GameState currentState = GameState.Menu;

    // Game settings
    public bool IsGameActive => currentState == GameState.Playing;

    void Awake()
    {
        // Singleton pattern
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        // Initialize game
        StartGame();
    }

    // Start the game
    public void StartGame()
    {
        currentState = GameState.Playing;
        Time.timeScale = 1f;
        Debug.Log("Game Started");
    }

    // Pause the game
    public void PauseGame()
    {
        if (currentState == GameState.Playing)
        {
            currentState = GameState.Paused;
            Time.timeScale = 0f;
            Debug.Log("Game Paused");
        }
    }

    // Resume the game
    public void ResumeGame()
    {
        if (currentState == GameState.Paused)
        {
            currentState = GameState.Playing;
            Time.timeScale = 1f;
            Debug.Log("Game Resumed");
        }
    }

    // Toggle pause
    public void TogglePause()
    {
        if (currentState == GameState.Playing)
        {
            PauseGame();
        }
        else if (currentState == GameState.Paused)
        {
            ResumeGame();
        }
    }

    // Game Over
    public void GameOver()
    {
        currentState = GameState.GameOver;
        Time.timeScale = 0f;
        Debug.Log("Game Over");
        // Add game over UI logic here
    }

    // Win condition
    public void Win()
    {
        currentState = GameState.Win;
        Time.timeScale = 0f;
        Debug.Log("You Won!");
        // Add win UI logic here
    }

    // Restart the game
    public void RestartGame()
    {
        currentState = GameState.Playing;
        Time.timeScale = 1f;
        // Reload the current scene or reset game objects
        Debug.Log("Game Restarted");
        // UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
    }

    // Get current game state
    public GameState GetGameState()
    {
        return currentState;
    }

    // Reset to menu
    public void ReturnToMenu()
    {
        currentState = GameState.Menu;
        Time.timeScale = 1f;
        Debug.Log("Returned to Menu");
    }

    void Update()
    {
        // Handle pause input (ESC key)
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
    }
}
