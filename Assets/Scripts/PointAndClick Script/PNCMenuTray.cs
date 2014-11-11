using System;
using UnityEngine;

public class PNCMenuTray
{
    public GameObject obj;
    public Vector2 position;
    public Vector2[] slots = new Vector2[6];
    float animVal;
    Vector2 size;
    
    public PNCMenuTray(Transform parent, GameObject g, Vector3 size)
    {
        obj = g;
        BoxCollider2D col = obj.GetComponent<BoxCollider2D>();
        obj.transform.localScale = Vector3.one;
        col.center = new Vector2(0, -0.25f);
        obj.name = "Tray";
        obj.transform.parent = parent;
        SpriteRenderer sr = obj.GetComponent<SpriteRenderer>();
        this.size = new Vector2(sr.sprite.bounds.size.x, sr.sprite.bounds.size.y);
        for (int i = 0; i < slots.Length; i++)
            slots [i] = new Vector2();
    }
    
    public void Animate(Camera cam, bool mouseOver)
    {
        BoxCollider2D col = obj.GetComponent<BoxCollider2D>();
        if (mouseOver)
            animVal = Mathf.Min(0.15f, animVal + Time.unscaledDeltaTime);
        else
            animVal = Mathf.Max(-0.1f, animVal - Time.unscaledDeltaTime);
        Vector2 p = cam.ScreenToWorldPoint(new Vector3(0.5f * Screen.width, animVal * Screen.height, 10));
        position = new Vector2(p.x, p.y);
        obj.transform.position = p;
        Vector3 s = StretchedScalar(size.x, size.y, cam);
        s = new Vector3(s.x, s.y * 0.3f, 1);
        obj.transform.localScale = s;
        col.size = new Vector2(size.x, size.y * 2);
        for (int i = 0; i < slots.Length; i++)
        {
            Vector2 v = cam.ViewportToWorldPoint(new Vector3((i + 1) * ((float)1 / (slots.Length + 1)), 0, 10));
            slots [i] = new Vector2(v.x, position.y);
        }
    }
    public Vector3 StretchedScalar(float sWidth, float sHeight, Camera cam)
    {
        float h = cam.orthographicSize * 2.0f;
        float w = h / Screen.height * Screen.width;
        return new Vector3(w / sWidth, h / sHeight, 1);
    }
}

