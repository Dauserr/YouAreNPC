using UnityEngine;

public class CarObstacle : Obstacle
{
    [Header("Car Settings")]
    public float moveSpeed = 5f;
    public Vector2 moveDirection = Vector2.left;
    public bool moveHorizontally = true;
    public float despawnDistance = 20f; // Distance before despawning

    private Vector3 initialPosition;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        initialPosition = transform.position;
        spriteRenderer = GetComponent<SpriteRenderer>();
        OnSpawn();
    }

    void Update()
    {
        // Move the car
        if (isActive)
        {
            MoveCar();
            CheckDespawn();
        }
    }

    void MoveCar()
    {
        // Move the car based on direction
        if (moveHorizontally)
        {
            transform.position += new Vector3(moveDirection.x * moveSpeed * Time.deltaTime, 0, 0);
        }
        else
        {
            transform.position += new Vector3(0, moveDirection.y * moveSpeed * Time.deltaTime, 0);
        }
    }

    void CheckDespawn()
    {
        // Despawn if too far from spawn point
        float distance = Vector3.Distance(transform.position, initialPosition);
        if (distance > despawnDistance)
        {
            OnDestroy();
        }
    }

    protected override void OnObstacleHit(HealthSystem npcHealth)
    {
        base.OnObstacleHit(npcHealth);
        
        // Knockback effect
        if (npcHealth != null)
        {
            Rigidbody2D npcRb = npcHealth.GetComponent<Rigidbody2D>();
            if (npcRb != null)
            {
                Vector2 knockback = (npcHealth.transform.position - transform.position).normalized * 5f;
                npcRb.AddForce(knockback, ForceMode2D.Impulse);
            }
        }
    }

    // Set car direction
    public void SetDirection(Vector2 direction)
    {
        moveDirection = direction.normalized;
    }

    // Set car speed
    public void SetSpeed(float speed)
    {
        moveSpeed = speed;
    }
}
