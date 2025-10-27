using UnityEngine;
using System.Collections;

public class ManholeTrap : Obstacle
{
    [Header("Manhole Settings")]
    public float openCloseInterval = 3f; // Time between open/close
    public float openDuration = 1.5f; // How long it stays open
    public float closeDuration = 0.5f; // How long closing animation takes

    [Header("Visual")]
    public SpriteRenderer coverSprite; // Optional cover sprite
    public Color closedColor = Color.white;
    public Color openColor = Color.red;

    private bool isOpen = false;
    private Collider2D trapCollider;

    void Start()
    {
        trapCollider = GetComponent<Collider2D>();
        if (coverSprite == null)
            coverSprite = GetComponent<SpriteRenderer>();
        
        OnSpawn();
        StartCoroutine(TrapCycle());
    }

    IEnumerator TrapCycle()
    {
        while (true)
        {
            if (!isActive)
            {
                yield return new WaitForSeconds(0.1f);
                continue;
            }

            // Open the trap
            yield return StartCoroutine(OpenTrap());
            
            // Stay open
            yield return new WaitForSeconds(openDuration);
            
            // Close the trap
            yield return StartCoroutine(CloseTrap());
            
            // Stay closed
            yield return new WaitForSeconds(openCloseInterval - openDuration - closeDuration);
        }
    }

    IEnumerator OpenTrap()
    {
        isOpen = true;
        float elapsed = 0f;

        // Disable collision during opening animation
        if (trapCollider != null)
            trapCollider.enabled = false;

        // Update visual
        while (elapsed < closeDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / closeDuration;
            
            if (coverSprite != null)
            {
                coverSprite.color = Color.Lerp(closedColor, openColor, t);
            }
            
            yield return null;
        }

        // Disable collision when fully open
        if (trapCollider != null)
            trapCollider.enabled = false;
    }

    IEnumerator CloseTrap()
    {
        isOpen = false;
        float elapsed = 0f;

        // Enable collision during closing animation
        if (trapCollider != null)
            trapCollider.enabled = true;

        // Update visual
        while (elapsed < closeDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / closeDuration;
            
            if (coverSprite != null)
            {
                coverSprite.color = Color.Lerp(openColor, closedColor, t);
            }
            
            yield return null;
        }
    }

    protected override void OnObstacleHit(HealthSystem npcHealth)
    {
        if (!isOpen) return; // Only deal damage when open
        base.OnObstacleHit(npcHealth);
    }

    public bool IsOpen()
    {
        return isOpen;
    }
}
