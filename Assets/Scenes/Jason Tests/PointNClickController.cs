using UnityEngine;
using System.Collections;

public class PointNClickController : MonoBehaviour
{

    public GameObject[] Others;
    public GameObject[] Neutrals;
    public GameObject[] Environmentals;
    public GameObject[] Responses;
    Camera cam;
    MenuTray tray;

    void Start()
    {
        tray = new MenuTray(this.transform);
        cam = this.GetComponentInParent<Camera>();
    }
    
    void Update()
    {
        TrayUpdate();
    }

    void TrayUpdate()
    {
        BoxCollider2D col = tray.obj.GetComponent<BoxCollider2D>();

        Vector2 mp = cam.ScreenToWorldPoint(Input.mousePosition + new Vector3(0,0,10));
        bool mouseOver = col.OverlapPoint(mp);
        tray.Animate(cam, mouseOver);
    }

    void OnDrawGizmos()
    {
        for (int i = 0; i < tray.slots.Length; i++)
            Gizmos.DrawIcon(tray.slots [i].ToVector3(), "icon.tiff");
    }
}

public class MenuTray
{
    public GameObject obj;
    public Vector2[] slots = new Vector2[6];
    float animVal;
    public MenuTray(Transform parent)
    {
        obj = new GameObject();
        BoxCollider2D col = obj.AddComponent<BoxCollider2D>();
        col.isTrigger = true;
        obj.name = "Tray";
        obj.transform.parent = parent;
        for (int i = 0; i < slots.Length; i++)
            slots [i] = new Vector2();

    }

    public void Animate(Camera cam, bool mouseOver)
    {
        BoxCollider2D col = obj.GetComponent<BoxCollider2D>();
        Vector2 p = cam.ViewportToWorldPoint(new Vector3(0, 0.5f, 10));
        if (mouseOver)
            animVal = Mathf.Min(0.005f, animVal + Time.unscaledDeltaTime);
        else
            animVal = Mathf.Max(0.001f, animVal - Time.unscaledDeltaTime);
        col.size = new Vector2(Screen.width * animVal, Screen.height * 0.1f);
        obj.transform.position = p;

        for (int i = 0; i < slots.Length; i++)
        {
            p = cam.ViewportToWorldPoint(new Vector3(-0.04f, (i + 1) * ((float)1 / (slots.Length + 1)), 10));
            slots[i] = p + new Vector2(col.size.x * 0.4f, 0);
        }

    }
}