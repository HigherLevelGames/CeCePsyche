using UnityEngine;
using System;

public class HoverAndHighlight : MonoBehaviour
{
    float value, a;
    bool glow;

    void Update()
    {
        Vector2 mp = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        glow = collider2D.bounds.Contains(mp);
        if (glow)
        {
            a += Time.deltaTime * 3;
            if (a > Mathf.PI)
                a -= Mathf.PI * 2;
            value = Mathf.Sin(a);
            renderer.material.SetFloat("_GlowRate", value);
        } else
        {
            if (value > -0.1f)
            {
                a -= Time.deltaTime * 3;
                value = Mathf.Sin(a);
                renderer.material.SetFloat("_GlowRate", value);
            }

        }
    }
}