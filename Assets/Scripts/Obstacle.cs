using UnityEngine;

public class Obstacle : MonoBehaviour
{
    [Header("Obstacle Settings")]
    public int damage = 10;          // Damage dealt to NPCs
    public float contactCooldown = 1f; // Time before can damage again
    public bool isActive = true;

    protected float lastDamageTime = 0f;

    // Check if obstacle can deal damage
    protected bool CanDamage()
    {
        if (!isActive) return false;
        return Time.time - lastDamageTime >= contactCooldown;
    }

    // Virtual method for collision handling (override in child classes)
    protected virtual void OnTriggerEnter2D(Collider2D other)
    {
        if (!CanDamage()) return;

        // Check if NPC entered the obstacle
        HealthSystem npcHealth = other.GetComponent<HealthSystem>();
        if (npcHealth != null)
        {
            npcHealth.TakeDamage(damage);
            lastDamageTime = Time.time;
            OnObstacleHit(npcHealth);
        }
    }

    // Override this method in child classes for specific behavior
    protected virtual void OnObstacleHit(HealthSystem npcHealth)
    {
        Debug.Log($"{gameObject.name} hit NPC for {damage} damage");
    }

    // Method to activate/deactivate obstacle
    public void SetActive(bool active)
    {
        isActive = active;
    }

    // Method called when obstacle is spawned
    public virtual void OnSpawn()
    {
        isActive = true;
    }

    // Method called when obstacle should be destroyed
    public virtual void OnDestroy()
    {
        Destroy(gameObject);
    }
}
