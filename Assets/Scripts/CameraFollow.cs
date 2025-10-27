using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("Target Settings")]
    public Transform target; // John-MC transform
    
    [Header("Camera Settings")]
    public float followSpeed = 2f; // How fast camera catches up
    public Vector3 offset = new Vector3(0, 0, -10f); // Camera position offset
    public bool snapOnStart = true; // Instantly move to target on start
    public bool followHorizontally = true; // Follow on X axis
    public bool followVertically = false; // Follow on Y axis
    public bool lockZ = true; // Keep Z position fixed (for 2D)
    
    [Header("Bounds (Optional)")]
    public bool useBounds = false;
    public float minX = -10f;
    public float maxX = 10f;
    public float minY = -10f;
    public float maxY = 10f;

    void Start()
    {
        if (target == null)
        {
            // Try to find John-MC
            GameObject johnMC = GameObject.FindGameObjectWithTag("Player");
            if (johnMC != null)
            {
                target = johnMC.transform;
            }
        }

        // Snap to target position on start if enabled
        if (snapOnStart && target != null)
        {
            transform.position = target.position + offset;
        }
    }

    void LateUpdate()
    {
        if (target == null) return;

        Vector3 targetPosition = target.position + offset;
        Vector3 desiredPosition = transform.position;
        
        // Only follow horizontally (X axis)
        if (followHorizontally)
        {
            desiredPosition.x = targetPosition.x;
        }
        
        // Only follow vertically (Y axis) if enabled
        if (followVertically)
        {
            desiredPosition.y = targetPosition.y;
        }
        
        // Keep Z position locked (for 2D orthographic cameras)
        if (lockZ)
        {
            desiredPosition.z = offset.z;
        }
        
        // Apply bounds if enabled
        if (useBounds)
        {
            desiredPosition.x = Mathf.Clamp(desiredPosition.x, minX, maxX);
            desiredPosition.y = Mathf.Clamp(desiredPosition.y, minY, maxY);
        }

        // Smoothly follow target
        transform.position = Vector3.Lerp(transform.position, desiredPosition, followSpeed * Time.deltaTime);
    }

    // Set camera target manually
    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }

    // Set follow speed
    public void SetFollowSpeed(float speed)
    {
        followSpeed = speed;
    }

    // Set camera offset
    public void SetOffset(Vector3 newOffset)
    {
        offset = newOffset;
    }
}

