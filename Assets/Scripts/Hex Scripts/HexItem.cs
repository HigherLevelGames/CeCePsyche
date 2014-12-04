using System;
using UnityEngine;

public class HexItem
{
    public Vector2 Target;
    public string name {
        get { return g.name; }
        set { g.name = value; } 
    }
    bool reverse;
    float glowValue;
    GameObject g;
    SpriteRenderer spr;

    public HexItem(GameObject g)
    {
        this.g = GameObject.Instantiate(g) as GameObject;
        //this.g.transform.parent = PsyData.parent;
        this.spr = this.g.GetComponent<SpriteRenderer>();
        this.Target = this.g.transform.position;
    }
            
    public void Scale(float scalar)
    {
        g.transform.localScale = new Vector3(scalar, scalar, 1);
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
