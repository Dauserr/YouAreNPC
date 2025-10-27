using UnityEngine;

public class Boundary : MonoBehaviour
{
    [Header("Boundary Settings")]
    public Collider2D boundaryCollider;
    public bool isTrigger = true; // Use as trigger or solid wall
    
    [Header("Boundary Type")]
    public bool blockLeft = false;     // Block movement to the left
    public bool blockRight = false;    // Block movement to the right
    public bool blockUp = false;       // Block movement up
    public bool blockDown = false;     // Block movement down
    
    [Header("Visual")]
    public Color gizmoColor = Color.red;
    public bool showInEditor = true;

    void Start()
    {
        // Get or add collider
        if (boundaryCollider == null)
        {
            boundaryCollider = GetComponent<Collider2D>();
            
            if (boundaryCollider == null)
            {
                // Add BoxCollider2D by default
                BoxCollider2D boxCollider = gameObject.AddComponent<BoxCollider2D>();
                boundaryCollider = boxCollider;
            }
        }
        
        // Set as trigger if specified
        boundaryCollider.isTrigger = isTrigger;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (isTrigger)
        {
            BlockCharacter(other.gameObject);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (!isTrigger)
        {
            BlockCharacter(collision.gameObject);
        }
    }

    void BlockCharacter(GameObject character)
    {
        // Get character's movement component
        Rigidbody2D rb = character.GetComponent<Rigidbody2D>();
        if (rb == null) return;

        // Calculate boundary limits
        Vector3 bounds = boundaryCollider.bounds.extents;
        Vector3 center = boundaryCollider.bounds.center;

        float leftLimit = center.x - bounds.x;
        float rightLimit = center.x + bounds.x;
        float topLimit = center.y + bounds.y;
        float bottomLimit = center.y - bounds.y;

        Vector3 characterPos = character.transform.position;
        Vector3 newPos = characterPos;

        // Clamp position based on boundary settings
        if (blockLeft && characterPos.x < center.x)
        {
            newPos.x = Mathf.Max(characterPos.x, leftLimit);
        }
        if (blockRight && characterPos.x > center.x)
        {
            newPos.x = Mathf.Min(characterPos.x, rightLimit);
        }
        if (blockUp && characterPos.y > center.y)
        {
            newPos.y = Mathf.Min(characterPos.y, topLimit);
        }
        if (blockDown && characterPos.y < center.y)
        {
            newPos.y = Mathf.Max(characterPos.y, bottomLimit);
        }

        // Apply boundary limits
        character.transform.position = newPos;

        // Stop velocity in blocked direction
        if (blockLeft && rb.linearVelocity.x < 0)
        {
            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
        }
        if (blockRight && rb.linearVelocity.x > 0)
        {
            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
        }
        if (blockUp && rb.linearVelocity.y > 0)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0);
        }
        if (blockDown && rb.linearVelocity.y < 0)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0);
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        // Continuously enforce boundary
        if (isTrigger)
        {
            BlockCharacter(other.gameObject);
        }
    }

    void OnDrawGizmos()
    {
        if (!showInEditor) return;

        Collider2D col = boundaryCollider != null ? boundaryCollider : GetComponent<Collider2D>();
        
        if (col == null)
        {
            // Draw placeholder if no collider
            Gizmos.color = gizmoColor;
            Gizmos.DrawWireCube(transform.position, new Vector3(1, 1, 0));
            return;
        }

        // Draw boundary
        Gizmos.color = gizmoColor;
        
        if (col is BoxCollider2D)
        {
            BoxCollider2D box = col as BoxCollider2D;
            Gizmos.DrawWireCube(col.bounds.center, col.bounds.size);
        }
        else if (col is CircleCollider2D)
        {
            CircleCollider2D circle = col as CircleCollider2D;
            float radius = circle.radius * Mathf.Max(transform.localScale.x, transform.localScale.y);
            Gizmos.DrawWireSphere(col.bounds.center, radius);
        }
    }
}

