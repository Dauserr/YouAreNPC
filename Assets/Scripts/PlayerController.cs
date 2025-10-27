using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public KeyCode upKey = KeyCode.W;
    public KeyCode downKey = KeyCode.S;
    public KeyCode leftKey = KeyCode.A;
    public KeyCode rightKey = KeyCode.D;

    [Header("Animation Settings")]
    public Animator animator;
    public string walkAnimationName = "Npc_walk";
    public string stayAnimationName = "Npc_stay";
    public string animationParameter = "Speed"; // Float parameter for blending

    [Header("Components")]
    public Rigidbody2D rb;
    public SpriteRenderer spriteRenderer;

    private Vector2 moveInput;
    private bool isMoving;

    void Start()
    {
        // Get components if not assigned
        if (rb == null)
            rb = GetComponent<Rigidbody2D>();
        
        if (animator == null)
            animator = GetComponent<Animator>();

        if (spriteRenderer == null)
            spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        // Check if GameManager exists
        if (GameManager.Instance != null && !GameManager.Instance.IsGameActive)
        {
            if (rb != null)
                rb.linearVelocity = Vector2.zero;
            isMoving = false;
            UpdateAnimations();
            return;
        }

        // Get input
        HandleInput();

        // Update animations
        UpdateAnimations();

        // Flip sprite based on movement
        FlipSprite();
    }

    void FixedUpdate()
    {
        // Check if GameManager exists
        if (GameManager.Instance != null && !GameManager.Instance.IsGameActive)
            return;

        // Apply movement
        MovePlayer();
    }

    void HandleInput()
    {
        moveInput = Vector2.zero;

        // Check WASD keys
        if (Input.GetKey(upKey) || Input.GetKey(KeyCode.UpArrow))
            moveInput.y += 1f;
        
        if (Input.GetKey(downKey) || Input.GetKey(KeyCode.DownArrow))
            moveInput.y -= 1f;
        
        if (Input.GetKey(leftKey) || Input.GetKey(KeyCode.LeftArrow))
            moveInput.x -= 1f;
        
        if (Input.GetKey(rightKey) || Input.GetKey(KeyCode.RightArrow))
            moveInput.x += 1f;

        // Normalize diagonal movement
        if (moveInput.magnitude > 1f)
        {
            moveInput = moveInput.normalized;
        }

        isMoving = moveInput.magnitude > 0.1f;
    }

    void MovePlayer()
    {
        if (rb != null)
        {
            Vector2 movement = moveInput * moveSpeed;
            rb.linearVelocity = movement;
        }
    }

    void FlipSprite()
    {
        if (spriteRenderer == null || moveInput.x == 0)
            return;

        // Flip sprite based on horizontal movement
        spriteRenderer.flipX = moveInput.x < 0;
    }

    void UpdateAnimations()
    {
        if (animator == null)
            return;

        // Check if Animator has a controller assigned
        if (animator.runtimeAnimatorController == null)
            return;

        float speed = rb != null ? rb.linearVelocity.magnitude : 0f;

        // Set speed parameter for animator blending
        // This allows smooth transition between Walk and Stay animations
        animator.SetFloat(animationParameter, speed);

        // Set directional animation parameters
        if (isMoving)
        {
            // Determine if moving vertically (up/down)
            bool movingVertical = Mathf.Abs(moveInput.y) > Mathf.Abs(moveInput.x);
            bool movingUp = moveInput.y > 0.3f;
            bool movingDown = moveInput.y < -0.3f;
            
            // Set animation parameters for directional movement
            animator.SetBool("WalkingUp", movingVertical && movingUp);
            animator.SetBool("WalkingDown", movingVertical && movingDown);
            animator.SetBool("WalkingHorizontal", !movingVertical);
        }
        else
        {
            // Not moving - reset directional parameters
            animator.SetBool("WalkingUp", false);
            animator.SetBool("WalkingDown", false);
            animator.SetBool("WalkingHorizontal", false);
        }
    }

    // Get current movement input
    public Vector2 GetMoveInput()
    {
        return moveInput;
    }

    // Check if player is moving
    public bool IsMoving()
    {
        return isMoving;
    }

    // Set move speed
    public void SetMoveSpeed(float speed)
    {
        moveSpeed = speed;
    }
}

