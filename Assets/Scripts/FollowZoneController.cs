using UnityEngine;
using System.Collections.Generic;

public class FollowZoneController : MonoBehaviour
{
    [Header("Zone Settings")]
    public float zoneRadius = 5f;
    public float damagePerSecond = 2f;
    public bool showZone = false; // Toggle visibility of zone
    public GameObject mainCharacter; // Reference to main character

    [Header("Player Lists")]
    public List<GameObject> npcsInZone = new List<GameObject>(); // Players in zone
    private List<GameObject> npcsOutsideZone = new List<GameObject>(); // Players outside zone

    [Header("Visual")]
    public LineRenderer zoneIndicator; // Optional visual indicator
    public bool useCircleRenderer = true; // Use circular zone renderer

    void Start()
    {
        // Find main character if not assigned
        if (mainCharacter == null)
        {
            mainCharacter = GameObject.FindGameObjectWithTag("Player");
        }

        // Set up zone indicator
        if (zoneIndicator != null && showZone)
        {
            DrawZoneIndicator();
        }
    }

    void Update()
    {
        CheckNPCsInZone();
        
        // Deal damage to players outside the zone
        foreach (GameObject npc in npcsOutsideZone)
        {
            if (npc != null)
            {
                HealthSystem npcHealth = npc.GetComponent<HealthSystem>();
                if (npcHealth != null && !npcHealth.IsDead())
                {
                    npcHealth.TakeDamage((int)(damagePerSecond * Time.deltaTime));
                }
            }
        }
    }

    void CheckNPCsInZone()
    {
        // Find all NPCs (or players to protect)
        GameObject[] npcs = GameObject.FindGameObjectsWithTag("Player");
        
        npcsInZone.Clear();
        npcsOutsideZone.Clear();

        foreach (GameObject npc in npcs)
        {
            if (npc == null) continue;

            float distance = Vector3.Distance(transform.position, npc.transform.position);

            if (distance <= zoneRadius)
            {
                npcsInZone.Add(npc);
            }
            else
            {
                npcsOutsideZone.Add(npc);
            }
        }
    }

    // Check if NPC is inside the zone
    public bool IsNPCInZone(GameObject npc)
    {
        float distance = Vector3.Distance(transform.position, npc.transform.position);
        return distance <= zoneRadius;
    }

    // Get NPCs inside zone
    public List<GameObject> GetNPCsInZone()
    {
        return npcsInZone;
    }

    // Get NPCs outside zone
    public List<GameObject> GetNPCsOutsideZone()
    {
        return npcsOutsideZone;
    }

    // Set zone radius
    public void SetZoneRadius(float newRadius)
    {
        zoneRadius = newRadius;
        if (zoneIndicator != null && showZone)
        {
            DrawZoneIndicator();
        }
    }

    // Draw zone indicator
    void DrawZoneIndicator()
    {
        if (zoneIndicator == null) return;

        int segments = 50;
        zoneIndicator.positionCount = segments + 1;
        zoneIndicator.useWorldSpace = false;
        zoneIndicator.loop = true;

        float angle = 0f;
        for (int i = 0; i < segments + 1; i++)
        {
            float x = Mathf.Sin(Mathf.Deg2Rad * angle) * zoneRadius;
            float y = Mathf.Cos(Mathf.Deg2Rad * angle) * zoneRadius;
            zoneIndicator.SetPosition(i, new Vector3(x, y, 0));
            angle += 360f / segments;
        }
    }

    // Visualize zone in editor
    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, zoneRadius);
    }

    // Update zone position to follow main character
    public void UpdateZonePosition()
    {
        if (mainCharacter != null)
        {
            transform.position = mainCharacter.transform.position;
        }
    }
}
