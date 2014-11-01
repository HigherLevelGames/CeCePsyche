using System;
using UnityEngine;

public class HexSlot
{
    public GameObject hex;
    
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

    public void SetActive(bool b)
    {
        this.hex.SetActive(b);
    }
}

public class PsyMenu : HexSlot
{
    public HexSlot[] Slots = new HexSlot[6];
    float scalar;
    bool cooling;

    public PsyMenu()
    {
        hex = GameObject.Instantiate(PsyData.menuhex) as GameObject;
        hex.transform.parent = PsyData.parent;
        for (int i = 0; i < Slots.Length; i++)
            Slots [i] = new HexSlot(hex.transform);
    }

    public void RoundMenuAnimate()
    {
        AnimTiming();
        float srad = 0.8f * scalar;
        float bonusRotation = Mathf.PI * 2 * scalar;
        for (int i = 0; i < Slots.Length; i++)
        {
            float r = ((float)i / Slots.Length) * Mathf.PI * 2 * scalar + bonusRotation;
            float x = srad * (Mathf.Cos(r) - Mathf.Sin(r));
            float y = srad * (Mathf.Sin(r) + Mathf.Cos(r));
            Slots [i].position = position + new Vector2(x, y);
            Slots [i].hex.transform.localScale = scalar * Vector3.one;
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
            Slots [i].hex.transform.localScale = scalar * Vector3.one;
        }
    }
    public void SoloAnimate()
    {
        AnimTiming();
    }
    void AnimTiming()
    {
        if (Active)
        {
            if (cooling)
            {
                scalar -= Time.unscaledDeltaTime;
                if (scalar < 0)
                    Close();
            } else if (scalar < 1)
                scalar = Mathf.Min(scalar + Time.unscaledDeltaTime, 1);
        }
        hex.transform.localScale = scalar * Vector3.one;
    }

    public void Close()
    {
        Active = false;
        cooling = false;
        scalar = 0;
    }

    public void Open()
    {
        Active = true;
        cooling = false;
    }

    public void Deselect()
    {
        cooling = true;
        hex.renderer.material.SetFloat("_GlowRate", -0.5f);
        for (int i = 0; i < Slots.Length; i++)
            Slots [i].hex.renderer.material.SetFloat("_GlowRate", -0.5f);
    }

    bool Active
    {
        get { return hex.activeSelf; }
        set
        {
            cooling = true;
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