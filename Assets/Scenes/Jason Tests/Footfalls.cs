using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Footfalls : MonoBehaviour
{
    public Sprite[] Sprites;
    public float Interval = 0.33f;
    public float LifeTime = 0.33f;
    private int current = 0;
    private float elapsedTime;
    private List<SpriteParticle> objs = new List<SpriteParticle>();

    void Start()
    {
        if (Sprites.Length < 1)
            Debug.Log("This script cannot function without sprites");

    }

    void Update()
    {
        elapsedTime += Time.deltaTime;
        if (elapsedTime > Interval)
        {
            SpriteParticle sp = new SpriteParticle(Sprites [current], LifeTime);

            objs.Add(sp);
            current++;
            if (current % Sprites.Length == 0)
                current = 0;
            elapsedTime = 0.0f;          
        }
        for (int i = 0; i < objs.Count; i++)
        {
            objs [i].Update();
            if (!objs [i].Exists)
            {
                objs [i].Destroy();
                objs.RemoveAt(i);
                i--;
            }
        }
    }
}

public class SpriteParticle
{
    float lifeTime = 1.0f;
    public bool Exists = true;
    GameObject obj;
    float totalLife = 1.0f;
    SpriteRenderer spr;

    public SpriteParticle(Sprite s, float life)
    {
        obj = new GameObject();
        spr = obj.AddComponent<SpriteRenderer>();
        spr.sprite = s;
        spr.sortingOrder = -1;
        spr.sortingLayerName = "Playground";
        totalLife = lifeTime = life;
    }

    public void Update()
    {
        lifeTime -= Time.deltaTime;
        if (lifeTime < 0)
            Exists = false;
        float a = Mathf.Sin((lifeTime / totalLife) * Mathf.PI);
        spr.color = new Color(1, 1, 1, a);
    }

    public void Destroy()
    {
        GameObject.DestroyImmediate(obj);
    }
}