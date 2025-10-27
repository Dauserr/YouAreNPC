using UnityEngine;

public class ZombieEnemy : Obstacle
{
    [Header("Movement Settings")]
    public float chaseSpeed = 2f;
    public float detectionRange = 10f;
    public float attackRange = 2f;
    public float attackCooldown = 2f;
    public int attackDamage = 20;

    [Header("Target")]
    public Transform target; // NPC to chase
    public string npcTag = "NPC";

    private Rigidbody2D rb;
    private Animator animator;
    private float lastAttackTime = 0f;
    private bool isChasing = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        // Find nearest NPC as target
        FindTarget();
    }

    void Update()
    {
        if (!target)
        {
            FindTarget();
        }

        if (target != null)
        {
            float distanceToTarget = Vector2.Distance(transform.position, target.position);

            if (distanceToTarget <= detectionRange)
            {
                isChasing = true;

                if (distanceToTarget <= attackRange)
                {
                    AttackTarget();
                }
                else
                {
                    ChaseTarget();
                }
            }
            else
            {
                isChasing = false;
                StopChasing();
            }
        }

        // Update animation
        UpdateAnimation();
    }

    void FindTarget()
    {
        // Find nearest NPC
        GameObject[] npcs = GameObject.FindGameObjectsWithTag(npcTag);
        float closestDistance = Mathf.Infinity;
        Transform closestNPC = null;

        foreach (GameObject npc in npcs)
        {
            if (npc == null) continue;

            HealthSystem npcHealth = npc.GetComponent<HealthSystem>();
            if (npcHealth == null || npcHealth.IsDead()) continue;

            float distance = Vector2.Distance(transform.position, npc.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestNPC = npc.transform;
            }
        }

        target = closestNPC;
    }

    void ChaseTarget()
    {
        if (target == null || rb == null) return;

        Vector2 direction = (target.position - transform.position).normalized;
        rb.linearVelocity = direction * chaseSpeed;

        // Face target
        if (direction.x != 0)
        {
            transform.localScale = new Vector3(Mathf.Sign(direction.x) * Mathf.Abs(transform.localScale.x),
                                                transform.localScale.y,
                                                transform.localScale.z);
        }
    }

    void AttackTarget()
    {
        if (target == null) return;
        if (Time.time - lastAttackTime < attackCooldown) return;

        lastAttackTime = Time.time;

        // Deal damage
        HealthSystem npcHealth = target.GetComponent<HealthSystem>();
        if (npcHealth != null && !npcHealth.IsDead())
        {
            npcHealth.TakeDamage(attackDamage);
            Debug.Log($"{gameObject.name} attacked {target.name} for {attackDamage} damage");
            lastDamageTime = Time.time; // Update base class damage time
        }
    }

    // Override base class collision for zombie-specific behavior
    protected override void OnObstacleHit(HealthSystem npcHealth)
    {
        // Zombies use continuous attack instead of trigger collisions
        // Damage is handled in AttackTarget() method
        Debug.Log($"{gameObject.name} is chasing {npcHealth.name}");
    }

    void StopChasing()
    {
        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
        }
    }

    void UpdateAnimation()
    {
        if (animator == null) return;

        animator.SetBool("IsChasing", isChasing);
        animator.SetFloat("Speed", rb != null ? rb.linearVelocity.magnitude : 0f);
    }

    void OnDrawGizmos()
    {
        // Draw detection range
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRange);

        // Draw attack range
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }

    // Set custom target
    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }
}
