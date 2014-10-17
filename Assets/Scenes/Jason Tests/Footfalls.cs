using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Footfalls : MonoBehaviour
{
    public Transform Origin;
    public Sprite[] Sprites;
    public bool Flip = false;
    public bool Grounded = false;
    public bool StepTrigger = false;

    public float StepSpriteInterval = 0.33f;
    public float StepSpriteLifeTime = 0.5f;
    private List<Footfall> footfalls = new List<Footfall>();

    void Start()
    {
        if (Sprites.Length < 1)
            Debug.Log("This script cannot function without sprites");
    }

    void Update()
    {
        if (Grounded)
        if (StepTrigger)
        {
            footfalls.Add(new Footfall(Sprites, StepSpriteInterval, Flip));
            StepTrigger = false;
        }

        for (int i = 0; i < footfalls.Count; i++)
        {
            footfalls [i].Update(StepSpriteLifeTime, Origin);
            if (!footfalls [i].Exists)
            {
                footfalls.RemoveAt(i);
                i--;
            }
        }
    }
    public class Footfall
    {
        public bool Exists = true;
        List<StepSprite> objs = new List<StepSprite>();
        Sprite[] sprites;
        float remainingTime;
        float time;
        int pastIdx = -1;
        bool flipped;
        
        public Footfall(Sprite[] sprs, float t, bool flip)
        {
            flipped = flip;
            time = remainingTime = t * (sprs.Length - 1);
            sprites = sprs;
        }
        
        public void Update(float spriteLife, Transform parent)
        {
            remainingTime -= Time.deltaTime;
            int idx = Mathf.FloorToInt((time - remainingTime) / time);
            if (idx < sprites.Length)
            {
                if (pastIdx < idx)
                {
                    objs.Add(new StepSprite(sprites [idx], spriteLife, flipped, parent));
                    pastIdx = idx;
                }
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
            if (idx > sprites.Length)
            {
                if (objs.Count == 0)
                    Exists = false;
            }
        }
    }
    
    public class StepSprite
    {
        public bool Exists = true;
        public bool Flipped = false;
        GameObject obj;
        SpriteRenderer spr;
        float lifeTime;
        float totalLife;
        
        public StepSprite(Sprite s, float life, bool flipped, Transform parent)
        {
            obj = new GameObject();
            obj.transform.position = parent.position;
            obj.transform.parent = parent;
            obj.name = "footfall";
            if (flipped)
                obj.transform.localScale = new Vector3(-1, 1, 1);
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
}