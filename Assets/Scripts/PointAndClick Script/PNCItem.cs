using System;
using UnityEngine;
public class PNCItem
{
    public ItemActions ActionID;
    public Vector2 Target;
    float glowValue;
    public GameObject obj;
    public bool Fired;
    SpriteRenderer spr;
    
    public PNCItem(GameObject g, ItemActions ia)
    {
        this.obj = g;
        this.spr = this.obj.GetComponentInChildren<SpriteRenderer>();
        this.ActionID = ia;
        this.Target = this.obj.transform.position;

    }
    public PNCItem(GameObject g)
    {
        this.obj = g;
        this.spr = this.obj.GetComponentInChildren<SpriteRenderer>();
        this.ActionID = ItemActions.Nothing;
        this.Target = this.obj.transform.position;
    }
    public void setCursor(GameObject g, ItemActions i)
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
        return obj.transform.GetChild(0).collider2D.bounds.Contains(p.ToVector3());
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
    public void Fire()
    {
        Fired = true;
    }
    public Vector2 position
    {
        get { return obj.transform.position.ToVector2(); }
        set { obj.transform.position = value; }
    }
}