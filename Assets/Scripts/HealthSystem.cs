using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    [Header("Health Settings")]
    public int maxHealth = 100;
    public float invincibilityDuration = 0.5f;

    private int currentHealth;
    private float invincibilityTimer = 0f;
    private bool isDead = false;

    // Events
    public System.Action<int> OnHealthChanged;
    public System.Action OnDeath;

    void Start()
    {
        currentHealth = maxHealth;
        if (OnHealthChanged != null)
            OnHealthChanged(currentHealth);
    }

    void Update()
    {
        // Update invincibility timer
        if (invincibilityTimer > 0)
        {
            invincibilityTimer -= Time.deltaTime;
        }
    }

    // Take damage
    public void TakeDamage(int damage)
    {
        if (isDead || invincibilityTimer > 0) return;

        currentHealth -= damage;
        currentHealth = Mathf.Max(0, currentHealth);
        invincibilityTimer = invincibilityDuration;

        if (OnHealthChanged != null)
            OnHealthChanged(currentHealth);

        if (currentHealth <= 0 && !isDead)
        {
            Die();
        }
    }

    // Heal
    public void Heal(int amount)
    {
        currentHealth += amount;
        currentHealth = Mathf.Min(maxHealth, currentHealth);
        
        if (OnHealthChanged != null)
            OnHealthChanged(currentHealth);
    }

    // Die
    void Die()
    {
        if (isDead) return;
        isDead = true;

        if (OnDeath != null)
            OnDeath();

        Debug.Log($"{gameObject.name} has died!");
    }

    // Check if dead
    public bool IsDead()
    {
        return isDead;
    }

    // Check if invincible
    public bool IsInvincible()
    {
        return invincibilityTimer > 0;
    }

    // Get current health
    public int GetHealth()
    {
        return currentHealth;
    }

    // Get health percentage (0-1)
    public float GetHealthPercentage()
    {
        return (float)currentHealth / maxHealth;
    }

    // Respawn or reset health
    public void ResetHealth()
    {
        currentHealth = maxHealth;
        isDead = false;
        invincibilityTimer = 0f;
        
        if (OnHealthChanged != null)
            OnHealthChanged(currentHealth);
    }

    // Set max health
    public void SetMaxHealth(int newMaxHealth)
    {
        maxHealth = newMaxHealth;
        currentHealth = Mathf.Min(currentHealth, maxHealth);
        
        if (OnHealthChanged != null)
            OnHealthChanged(currentHealth);
    }
}
