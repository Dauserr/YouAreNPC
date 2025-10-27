using UnityEngine;
using System;

[Serializable]
public class WaypointData
{
    public Transform waypoint;
    public float waitTime = 2f; // Seconds to wait at this waypoint
}

public class JohnMCTrajectoryController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 2f;
    
    [Header("Trajectory Points")]
    public WaypointData[] waypoints; // Set waypoints with wait times in Inspector

    [Header("Animation Settings")]
    public Animator animator;
    public string walkAnimationName = "JohnWalk";
    public string stayAnimationName = "Mn_stay";
    public string hitAnimationName = "Mn_hit";
    public string kickAnimationName = "Mn_kick";
    public string downAnimationName = "Mn_down";
    
    private int currentWaypointIndex = 0;
    private bool isMoving = false;
    private bool isAttacking = false;
    private bool isWaiting = false;
    private float waitTimer = 0f;
    private bool hasReachedLastWaypoint = false;

    void Start()
    {
        if (animator == null)
            animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (waypoints == null || waypoints.Length == 0)
            return;

        MoveAlongTrajectory();
        UpdateAnimations();
    }

    void MoveAlongTrajectory()
    {
        if (isAttacking) return; // Don't move while attacking
        
        // Check if reached last waypoint
        if (hasReachedLastWaypoint)
        {
            isMoving = false;
            isWaiting = false;
            return;
        }

        // Check if we're waiting at a waypoint
        if (isWaiting)
        {
            waitTimer -= Time.deltaTime;
            if (waitTimer <= 0)
            {
                isWaiting = false;
                currentWaypointIndex++;
                
                if (currentWaypointIndex >= waypoints.Length)
                {
                    hasReachedLastWaypoint = true;
                    isMoving = false;
                    return;
                }
            }
            else
            {
                return; // Still waiting
            }
        }

        if (currentWaypointIndex >= waypoints.Length)
        {
            hasReachedLastWaypoint = true;
            isMoving = false;
            return;
        }

        WaypointData currentWaypointData = waypoints[currentWaypointIndex];
        if (currentWaypointData == null || currentWaypointData.waypoint == null) return;
        
        Transform targetWaypoint = currentWaypointData.waypoint;

        // Move towards waypoint
        Vector2 direction = (targetWaypoint.position - transform.position).normalized;
        float distance = Vector2.Distance(transform.position, targetWaypoint.position);

        if (distance < 0.1f)
        {
            // Reached waypoint - start waiting (use this waypoint's wait time)
            isWaiting = true;
            waitTimer = currentWaypointData.waitTime;
            isMoving = false;
        }
        else
        {
            transform.position += (Vector3)direction * moveSpeed * Time.deltaTime;
            isMoving = true;
        }

        // Flip sprite based on direction
        if (Mathf.Abs(direction.x) > 0.1f)
        {
            SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
            if (spriteRenderer != null)
            {
                spriteRenderer.flipX = direction.x < 0;
            }
        }
    }

    void UpdateAnimations()
    {
        if (animator == null)
            return;

        if (animator.runtimeAnimatorController == null)
            return;

        // Set isMoving parameter (matches controller)
        animator.SetBool("IsMoving", isMoving);
        
        // Set speed for walking animations (matches controller)
        float speed = isMoving ? moveSpeed : 0f;
        animator.SetFloat("Speed", speed);
    }

    // Call this when John-MC should take a hit
    public void TakeHit()
    {
        if (animator == null || isAttacking) return;
        
        isAttacking = true;
        // Trigger the Hit trigger parameter (matches JohnController)
        animator.SetTrigger("HitTrigger");
    }

    // Call this when John-MC should kick
    public void PerformKick()
    {
        if (animator == null || isAttacking) return;
        
        isAttacking = true;
        // Trigger the Kick trigger parameter (matches JohnController)
        animator.SetTrigger("KickTrigger");
    }

    // Called when attack animation ends
    void OnAttackEnd()
    {
        isAttacking = false;
    }

    // Check if John-MC is attacking
    public bool IsAttacking()
    {
        return isAttacking;
    }
}

