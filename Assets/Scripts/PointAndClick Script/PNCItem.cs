using System;
using UnityEngine;

public class PNCItem
{

    public ItemActions ActionID;
    public GameObject obj;

    public Vector2 position
    {
        get { return obj.transform.position.ToVector2(); }
        set { obj.transform.position = value; }
    }

    public bool Active
    {
        get { return obj.activeSelf;}
        set { obj.SetActive(value); }
    }

    public bool Fired
    { 
        get { return Fireable ? Fireable.Fire : false;}
        set { if(Fireable) Fireable.Fire = value; }
    }


    SpriteRenderer spr;
    FireAction Fireable;
    float glowValue;

    public PNCItem(GameObject g, ItemActions ia)
    {
        this.obj = g;
        this.ActionID = ia;
        BaseInitialization();
    }

    public PNCItem(GameObject g)
    {
        this.obj = g;
        this.ActionID = ItemActions.Nothing;
        BaseInitialization();
    }

    void BaseInitialization()
    {
        Fireable = obj.GetComponent<FireAction>();
        spr = obj.GetComponentInChildren<SpriteRenderer>();
    }

    public bool IsMouseOver(Vector2 p)
    {
        return obj.collider2D.bounds.Contains(p);
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


}