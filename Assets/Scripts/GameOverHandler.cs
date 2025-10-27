using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class GameOverHandler : MonoBehaviour
{
    [Header("References")]
    public FollowZoneController zoneController;
    
    [Header("Game Over UI")]
    public GameObject gameOverPanel;
    public Text gameOverText;
    
    [Header("Win Conditions")]
    public int survivedMinutes = 2;
    public int npcSaveCount = 10; // Number of NPCs to save to win
    
    private int savedNpcCount = 0;
    private float gameStartTime;

    void Start()
    {
        gameStartTime = Time.time;
        
        // Subscribe to NPC death events
        GameObject[] npcs = GameObject.FindGameObjectsWithTag("NPC");
        foreach (GameObject npc in npcs)
        {
            HealthSystem health = npc.GetComponent<HealthSystem>();
            if (health != null)
            {
                health.OnDeath += OnNPCDied;
            }
        }
    }

    void Update()
    {
        if (GameManager.Instance == null || !GameManager.Instance.IsGameActive)
            return;

        // Check NPCs in zone
        CheckNPCsInDangerZone();
        
        // Check win condition
        CheckWinCondition();
    }

    void CheckNPCsInDangerZone()
    {
        if (zoneController == null) return;

        List<GameObject> npcsOutsideZone = zoneController.GetNPCsOutsideZone();
        
        // Check if NPCs have died from being outside zone
        foreach (GameObject npc in npcsOutsideZone)
        {
            if (npc == null) continue;
            
            HealthSystem npcHealth = npc.GetComponent<HealthSystem>();
            if (npcHealth != null && npcHealth.IsDead())
            {
                OnNPCLeftZone();
            }
        }

        // Check if all NPCs are dead
        if (AreAllNPCsDead())
        {
            TriggerGameOver("All NPCs have died!");
        }
    }

    bool AreAllNPCsDead()
    {
        GameObject[] npcs = GameObject.FindGameObjectsWithTag("NPC");
        
        foreach (GameObject npc in npcs)
        {
            HealthSystem health = npc.GetComponent<HealthSystem>();
            if (health != null && !health.IsDead())
            {
                return false;
            }
        }
        
        return npcs.Length > 0;
    }

    void CheckWinCondition()
    {
        float elapsedTime = Time.time - gameStartTime;
        
        // Win condition: Survive for X minutes
        if (elapsedTime >= survivedMinutes * 60f)
        {
            TriggerWin("Survival Time Achieved!");
        }
        
        // Alternative win condition: Save enough NPCs
        if (savedNpcCount >= npcSaveCount)
        {
            TriggerWin("Saved Enough NPCs!");
        }
    }

    void OnNPCDied()
    {
        GameObject[] npcs = GameObject.FindGameObjectsWithTag("NPC");
        int aliveCount = 0;
        
        foreach (GameObject npc in npcs)
        {
            HealthSystem health = npc.GetComponent<HealthSystem>();
            if (health != null && !health.IsDead())
            {
                aliveCount++;
            }
        }

        // Check if game should end
        if (aliveCount == 0)
        {
            TriggerGameOver("All NPCs are dead!");
        }
    }

    void OnNPCLeftZone()
    {
        Debug.Log("NPC left the zone!");
        // You can add specific handling here
    }

    void TriggerGameOver(string reason)
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.GameOver();
        }

        ShowGameOverUI(reason);
        
        Debug.Log($"Game Over: {reason}");
    }

    void TriggerWin(string reason)
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.Win();
        }

        ShowWinUI(reason);
        
        Debug.Log($"You Win: {reason}");
    }

    void ShowGameOverUI(string message)
    {
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
            
            if (gameOverText != null)
            {
                gameOverText.text = $"Game Over!\n{message}";
            }
        }
    }

    void ShowWinUI(string message)
    {
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
            
            if (gameOverText != null)
            {
                gameOverText.text = $"You Win!\n{message}";
            }
        }
    }

    public void IncrementSavedNPCs()
    {
        savedNpcCount++;
        Debug.Log($"NPCs saved: {savedNpcCount}/{npcSaveCount}");
    }

    // Reset handler
    public void Reset()
    {
        savedNpcCount = 0;
        gameStartTime = Time.time;
        
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(false);
        }
    }
}
