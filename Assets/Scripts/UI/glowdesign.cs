using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GlowPulse : MonoBehaviour
{
    private SpriteRenderer sr;
    private float baseAlpha;
    private float offset;
    private float pulseSpeed;
    
    // Store the starting Y position so it bobs around a center point
    private float startY;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        baseAlpha = sr.color.a;
        offset = Random.Range(0f, 100f);
        pulseSpeed = Random.Range(0.5f, 1.5f);
        
        startY = transform.position.y;
    }

    void Update()
    {
        // 1. PULSE THE ALPHA (Shimmering)
        float pulse = Mathf.Sin(Time.time * pulseSpeed + offset) * 0.05f;
        Color c = sr.color;
        c.a = Mathf.Clamp(baseAlpha + pulse, 0.05f, 0.3f);
        sr.color = c;

        // 2. VERTICAL DRIFT (Up and Down)
        // 0.5f is the speed of the bobbing
        // 0.2f is the height (how far up and down it goes)
        float newY = startY + Mathf.Sin(Time.time * 0.5f + offset) * 0.2f;
        // Rocks back and forth between -30 and 30 degrees
float angle = Mathf.Sin(Time.time * 0.5f + offset) * 30f;
transform.rotation = Quaternion.Euler(0, 0, angle); }
}