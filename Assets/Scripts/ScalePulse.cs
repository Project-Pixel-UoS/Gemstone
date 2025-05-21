using System;
using UnityEngine;
using Util;

public class ScalePulse : MonoBehaviour
{
    private Vector3 baseScale;
    private float targetFactor;
    private float speed;
    private float t;

    public void Init(Vector3 originalScale, float scaleMultiplier, float animationSpeed)
    {
        baseScale = originalScale;
        targetFactor = scaleMultiplier;
        speed = animationSpeed;
    }

    void Update()
    {
        t += Time.deltaTime * speed;
        transform.localScale = Vector3.Lerp(baseScale, baseScale * targetFactor, Mathf.PingPong(t * 2, 1f));

        if (t > 0.5f)
        {
            transform.localScale = baseScale; // Ensure exact reset
        }
    }
}
