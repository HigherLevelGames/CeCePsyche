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
    //private HMovementController horizontal;
    //private VMovementController vertical;
	private MovementController moveControl;
    void Start()
    {
        if (Sprites.Length < 1)
            Debug.Log("This script cannot function without sprites");
		moveControl = this.GetComponentInParent<MovementController>();
        //horizontal = this.GetComponentInParent<HMovementController>();
        //vertical = this.GetComponentInParent<VMovementController>();
    }

    void Update()
    {
		Flip = !moveControl.isFacingRight;//horizontal.isFacingRight;
		Grounded = moveControl.isGrounded;//vertical.isGrounded;
        if (Grounded)
        if (StepTrigger)
        {
            Vector2 p = Origin.position.ToVector2();
            footfalls.Add(new Footfall(Sprites, p, StepSpriteInterval, Flip));
            StepTrigger = false;
        }

        for (int i = 0; i < footfalls.Count; i++)
        {
            footfalls [i].Update(StepSpriteLifeTime);
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
        Vector2 position;
        public Footfall(Sprite[] sprs, Vector2 p, float t, bool flip)
        {
            position = p;
            flipped = flip;
            time = remainingTime = t * (sprs.Length - 1);
            sprites = sprs;
        }
        
        public void Update(float spriteLife)
        {
            remainingTime -= Time.deltaTime;
            int idx = Mathf.FloorToInt((time - remainingTime) / time);
            if (idx < sprites.Length)
            {
                if (pastIdx < idx)
                {
                    objs.Add(new StepSprite(sprites [idx], spriteLife, flipped, position));
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
        
        public StepSprite(Sprite s, float life, bool flipped, Vector2 p)
        {
            obj = new GameObject();
            obj.transform.position = p;
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