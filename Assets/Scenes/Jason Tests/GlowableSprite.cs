using UnityEngine;
using System.Collections;

public class GlowableSprite : MonoBehaviour
{
    public enum Method
    {
        None = 0,
        BehindSprite = 1,
        Sprite = 2,
        Both = 3
    }
    public SpriteRenderer TargetRenderer;
    public Method GlowMethod;
    public Material alternateMaterial;
    public float Value;

    GameObject backSprite;
    SpriteRenderer bspr;


    // Use this for initialization
    void Start()
    {
        if (!TargetRenderer)
        {
            Debug.Log("The script GlowableSprite is missing a public variable assignment.");
            return;
        }
        switch (GlowMethod)
        {
            case Method.None:
                return;
            case Method.BehindSprite:
                SetupRenderers();
                return;
            case Method.Sprite:
                return;
            case Method.Both:
                SetupRenderers();
                return;

        }


    }
    void SetupRenderers()
    {
        Sprite sp = TargetRenderer.sprite;
        backSprite = new GameObject();
        bspr = backSprite.AddComponent<SpriteRenderer>();
        bspr.color = new Color(1, 1, 1, 1);
        bspr.sprite = sp;
        bspr.material = alternateMaterial;
        bspr.sortingLayerID = TargetRenderer.sortingLayerID;
        bspr.sortingOrder = TargetRenderer.sortingOrder - 1;
        float oldWidth = sp.bounds.extents.x;
        float oldHeight = sp.bounds.extents.y;
        backSprite.name = "haloglow";
        backSprite.transform.parent = this.transform;
        backSprite.transform.position = new Vector3(this.transform.position.x - oldWidth * 0.05f, this.transform.position.y - oldHeight * 0.05f, 0);
        backSprite.transform.localScale = new Vector3(1.1f, 1.1f, 1f);
    }

    bool reverse;

    void Update()
    {
        if (!TargetRenderer)
            return;
        if (Value < 0)
            reverse = true;
        else if (Value > 1)
            reverse = false;
        if (reverse)
            Value += Time.deltaTime;
        else
            Value -= Time.deltaTime;
        switch (GlowMethod)
        {
            case Method.None:
                return;
            case Method.BehindSprite:
                bspr.color = new Color(1, 1, 1, Value);
                return;
            case Method.Sprite:
                TargetRenderer.material.SetFloat("_GlowRate", Value);
                return;
            case Method.Both:
                bspr.color = new Color(1, 1, 1, Value);
                TargetRenderer.material.SetFloat("_GlowRate", Value);
                return;
        }
    }
}
