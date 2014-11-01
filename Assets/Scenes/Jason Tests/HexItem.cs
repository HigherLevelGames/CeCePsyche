using System;
using UnityEngine;

public class HexItem
{
    public PMType Type;
    public Vector2 Target;
    bool reverse;
    float glowValue;
    GameObject g;
    SpriteRenderer spr;

    public HexItem(GameObject g, PMType t)
    {
        this.g = GameObject.Instantiate(g) as GameObject;
        this.g.transform.parent = PsyData.parent;
        this.spr = this.g.GetComponent<SpriteRenderer>();
        this.Type = t;
        this.Target = this.g.transform.position;
    }
            
    public void Scale(float scalar)
    {
        g.transform.localScale = Vector3.one * scalar;
    }
            
    public void Glow()
    {
        if (glowValue > 1)
            reverse = true;
        if (glowValue < 0)
            reverse = false;
        if (reverse)
            glowValue -= Time.unscaledDeltaTime;
        else
            glowValue += Time.unscaledDeltaTime;
        spr.material.SetFloat("_GlowRate", glowValue);
    }
            
    public void SetActive(bool val)
    {
        g.SetActive(val);
    }
            
    public Vector2 position
    {
        get { return g.transform.position.ToVector2(); }
        set { g.transform.position = value; }
    }
}
