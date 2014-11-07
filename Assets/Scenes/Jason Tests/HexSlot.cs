using System;
using UnityEngine;

public class HexSlot
{
    public GameObject hex;
    float glowValue;
    public Vector2 position
    {
        get { return p; }
        set
        {
            p = value;
            hex.transform.position = value;
        }
    }

    Vector2 p;
    public int ItemID = -1;

    public HexSlot(Transform t)
    {
        hex = GameObject.Instantiate(PsyData.menuhex) as GameObject;
        hex.transform.parent = t;
    }

    public HexSlot()
    {

    }
    public void Glow()
    {
        glowValue += Time.unscaledDeltaTime * 4;

        if (glowValue > Mathf.PI * 2)
            glowValue -= Mathf.PI * 2;

        hex.renderer.material.SetFloat("_GlowRate", Mathf.Sin(glowValue * 0.5f));
    }
    public void Select()
    {
        glowValue = 0.5f;
        hex.renderer.material.SetFloat("_GlowRate", glowValue);

    }
    public void Deselect()
    {
        glowValue = -0.2f;
        hex.renderer.material.SetFloat("_GlowRate", glowValue);
    }

    public void SetActive(bool b)
    {
        this.hex.SetActive(b);
    }
}

public class PsyMenu : HexSlot
{
    public HexSlot[] Slots;
    float scalar;
    float itemScalar;
    bool cooling;
    bool itemCooling;

    public PsyMenu(int numSlots)
    {
        Slots = new HexSlot [numSlots];
        hex = GameObject.Instantiate(PsyData.menuhex) as GameObject;
        hex.transform.parent = PsyData.parent;
        for (int i = 0; i < Slots.Length; i++)
        {
            Slots [i] = new HexSlot(hex.transform);
            Slots[i].hex.name = "Slot " + i.ToString();
        }
    }

    public void RoundMenuAnimate()
    {
        AnimTiming();
        float srad = 0.8f * itemScalar;
        float bonusRotation = Mathf.PI * 4 * itemScalar;
        for (int i = 0; i < Slots.Length; i++)
        {
            float r = ((float)i / Slots.Length) * Mathf.PI * 2 * itemScalar + bonusRotation;
            float x = srad * (Mathf.Cos(r) - Mathf.Sin(r));
            float y = srad * (Mathf.Sin(r) + Mathf.Cos(r));
            Slots [i].position = position + new Vector2(x, y);
            Slots [i].hex.transform.localScale = itemScalar * Vector3.one;
        }
    }

    public void BoxMenuAnimate()
    {
        AnimTiming();
        for (int i = 0; i < Slots.Length; i++)
        {
            float x = (float)i + 1;
            float y = (i % 2) * 0.5f - 0.5f;

            Slots [i].position = position + new Vector2(x, y);
            Slots [i].hex.transform.localScale = itemScalar * Vector3.one;
        }
    }
    void AnimTiming()
    {
        if (Active)
        {
            if (cooling)
            {
                scalar -= Time.unscaledDeltaTime * 2;
                if (scalar < 0)
                    Close();
            } else if (scalar < 1)
                scalar = Mathf.Min(scalar + Time.unscaledDeltaTime, 1);
            if(itemCooling)
                itemScalar = Mathf.Max(itemScalar - Time.unscaledDeltaTime * 2, 0);
            else if( itemScalar < 1)
                itemScalar = Mathf.Min(itemScalar + Time.unscaledDeltaTime, 1);
        }
        hex.transform.localScale = scalar * Vector3.one;
    }

    public void Close()
    {
        Active = false;
        cooling = false;
        itemCooling = false;
        scalar = 0;
        itemScalar = 0;
        hex.renderer.material.SetFloat("_GlowRate", -0.2f);
        for (int i = 0; i < Slots.Length; i++)
            Slots [i].hex.renderer.material.SetFloat("_GlowRate", -0.2f);
    }

    public void Open()
    {
        Active = true;
        itemCooling = false;
        cooling = false;
    }

    public void Cool()
    {
        cooling = true;
        Deselect();
    }
    public new void Deselect()
    {
        itemCooling = true;
        hex.renderer.material.SetFloat("_GlowRate", -0.2f);
        hex.renderer.sortingOrder = 10002;
        for (int i = 0; i < Slots.Length; i++)
        {
            Slots [i].hex.renderer.material.SetFloat("_GlowRate", -0.2f);
            Slots[i].hex.renderer.sortingOrder = 10002;
        }
    }
    public new void Select()
    {
        Active = true;
        cooling = false;
        itemCooling = false;
        SortTo(10003);
        hex.renderer.material.SetFloat("_GlowRate", 0.5f);
    }
    public void SortTo(int val)
    {
        hex.renderer.sortingOrder = val;
        for (int i = 0; i < Slots.Length; i++)
            Slots[i].hex.renderer.sortingOrder = val;
    }
    bool Active
    {
        get { return hex.activeSelf; }
        set
        {
            hex.SetActive(value);
            for (int i = 0; i < Slots.Length; i++)
                Slots [i].SetActive(value);
        }
    }
    /*
    void Glow()
    {
        glowval += Time.unscaledDeltaTime * 5;
        if (glowval > Mathf.PI * 2.0f)
            glowval -= Mathf.PI * 2.0f;
        float n = Mathf.Sin(glowval) * 0.5f;
        Debug.Log(n);
        switch (stage)
        {
            case MenuStage.Target:
                targets[rTarget].hex.renderer.material.SetFloat("_GlowRate", n);
                for(int i = 0; i < targets[rTarget].Slots.Length; i++)
                {
                    targets[rTarget].Slots[i].hex.renderer.material.SetFloat("_GlowRate", n);
                }
                break;
        }
    }*/
}