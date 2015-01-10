using UnityEngine;
using System.Collections;

public class AlphaGlowSprite : MonoBehaviour
{
    public float SecondsToFade = 1.0f;
    SpriteRenderer spr;
    float frame;
    float oneoverseconds;

    void Start()
    {
        spr = this.GetComponent<SpriteRenderer>();
        frame = SecondsToFade;
        oneoverseconds = (float)1 / SecondsToFade;
    }
        
    void Update()
    {
        frame += Time.deltaTime;
        if (frame > SecondsToFade)
            frame -= SecondsToFade;
        float a = frame * oneoverseconds;
        float newAlpha = Mathf.Sin(a * Mathf.PI);
        Color c = spr.color;
        c.a = newAlpha;
        spr.color = c;
    }
}
