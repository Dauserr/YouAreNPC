using UnityEngine;

public class CircleZoneRenderer : MonoBehaviour
{
    [Header("Zone Settings")]
    public float zoneRadius = 5f;
    public Color zoneColor = new Color(1f, 1f, 0f, 0.3f); // Yellow with transparency
    public int circleSegments = 50; // How smooth the circle is
    
    [Header("Visual Settings")]
    public bool showZone = true;
    public Material lineMaterial;

    private LineRenderer lineRenderer;

    void Start()
    {
        if (!showZone) return;

        // Create or get LineRenderer
        lineRenderer = GetComponent<LineRenderer>();
        if (lineRenderer == null)
        {
            lineRenderer = gameObject.AddComponent<LineRenderer>();
        }

        // Configure LineRenderer
        lineRenderer.useWorldSpace = false;
        lineRenderer.startWidth = 0.1f;
        lineRenderer.endWidth = 0.1f;
        lineRenderer.material = lineMaterial != null ? lineMaterial : CreateDefaultMaterial();
        lineRenderer.startColor = zoneColor;
        lineRenderer.endColor = zoneColor;
        lineRenderer.loop = true;

        // Draw circle
        DrawCircle();
    }

    void Update()
    {
        // Update circle if radius changed
        if (showZone && lineRenderer != null)
        {
            DrawCircle();
        }
    }

    void DrawCircle()
    {
        if (lineRenderer == null) return;

        lineRenderer.positionCount = circleSegments + 1;

        for (int i = 0; i <= circleSegments; i++)
        {
            float angle = (2 * Mathf.PI / circleSegments) * i;
            float x = Mathf.Cos(angle) * zoneRadius;
            float z = Mathf.Sin(angle) * zoneRadius;
            lineRenderer.SetPosition(i, new Vector3(x, 0, z));
        }
    }

    Material CreateDefaultMaterial()
    {
        Material mat = new Material(Shader.Find("Sprites/Default"));
        mat.color = zoneColor;
        return mat;
    }

    void OnDrawGizmos()
    {
        if (!showZone) return;

        Gizmos.color = new Color(zoneColor.r, zoneColor.g, zoneColor.b, 0.5f);
        Gizmos.DrawWireSphere(transform.position, zoneRadius);
    }

    // Change zone radius at runtime
    public void SetZoneRadius(float newRadius)
    {
        zoneRadius = newRadius;
    }

    // Change zone color at runtime
    public void SetZoneColor(Color newColor)
    {
        zoneColor = newColor;
        if (lineRenderer != null)
        {
            lineRenderer.startColor = newColor;
            lineRenderer.endColor = newColor;
        }
    }

    // Toggle zone visibility
    public void ToggleZone(bool visible)
    {
        showZone = visible;
        if (lineRenderer != null)
        {
            lineRenderer.enabled = visible;
        }
    }
}

