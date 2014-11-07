using System;
using UnityEngine;
public class PMenuItem
{
    public int ActionID;
    public Vector2 Target;
    float glowValue;
    public GameObject obj;
    SpriteRenderer spr;
    
    public PMenuItem(GameObject g, int i)
    {
        this.obj = g;
        this.spr = this.obj.GetComponent<SpriteRenderer>();
        this.ActionID = i;
        this.Target = this.obj.transform.position;

    }
    public PMenuItem(GameObject g)
    {
        this.obj = g;
        this.spr = this.obj.GetComponent<SpriteRenderer>();
        this.ActionID = -1;
        this.Target = this.obj.transform.position;
    }
    public void setCursor(GameObject g, int i)
    {
        GameObject.DestroyImmediate(this.obj);
        this.ActionID = i;
        this.obj = GameObject.Instantiate(g) as GameObject;
        this.obj.transform.localScale = Vector3.one;
    }
    public void Scale(float scalar)
    {
        obj.transform.localScale = new Vector3(scalar, scalar, 1);
    }
    public bool IsMouseOver(Vector2 p)
    {
        return obj.collider2D.bounds.Contains(p.ToVector3());
    }
    public void Glow()
    {
        glowValue += Time.deltaTime * 4;
        if (glowValue > Mathf.PI * 2)
            glowValue -= Mathf.PI * 2;
        float a = Mathf.Sin(glowValue) + 0.9f;
        spr.material.SetFloat("_GlowRate", a);
    }
    public void Mute()
    {
        if (glowValue > 0)
        {
            glowValue = 0;
            spr.material.SetFloat("_GlowRate", glowValue);
        }
    }
    public void SetActive(bool val)
    {
        obj.SetActive(val);
    }
    
    public Vector2 position
    {
        get { return obj.transform.position.ToVector2(); }
        set { obj.transform.position = value; }
    }
}