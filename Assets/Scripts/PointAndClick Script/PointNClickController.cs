using UnityEngine;
using System.Collections;

public class PointNClickController : MonoBehaviour
{
    public GameObject ConditionableCharacter;
    public MovementController HController;
    public GameObject[] Others;
    public GameObject[] Neutrals;
    public GameObject[] Environmentals;
    public GameObject[] Responses;
    public GameObject MouseIcon;
    public GameObject Tray;
    public GameObject TimeOver;

    Camera cam;
    MenuTray tray;
    PMenuItem[] items = new PMenuItem[6];
    PMenuItem cursor;
    Vector2 WalkToTarget;
    float totalSeconds, secondsRemaining;
    public ParticleSystem TimerPS;
    GameObject TOScreen;
    void Start()
    {

        WalkToTarget = ConditionableCharacter.transform.position;
        cam = this.GetComponentInParent<Camera>();
        Vector3 s = cam.ScreenToWorldPoint(new Vector3(Screen.width * 0.9f, Screen.height * 0.8f, 20));
        tray = new MenuTray(this.transform,Instantiate(Tray) as GameObject, s);
        GameObject m = Instantiate(MouseIcon) as GameObject;
        cursor = new PMenuItem(m);
        for (int i = 0; i < items.Length; i++)
        {
            GameObject g = Instantiate(Neutrals[i % Neutrals.Length]) as GameObject;
            g.transform.parent = tray.obj.transform;
            g.name = "slot " + i.ToString();
            items [i] = new PMenuItem(g, i);
            items[i].SetActive(true);
        }
        HController = ConditionableCharacter.GetComponent<MovementController>();

        totalSeconds = secondsRemaining = 30.0f;
    }
    bool mouseInTray = false;
    void Update()
    {
        Vector2 mp = cam.ScreenToWorldPoint(Input.mousePosition + new Vector3(0, 0, 10));
        Vector2 p = ConditionableCharacter.transform.position;
        cursor.position = mp;
        if(Input.GetMouseButtonDown(0))
            ClickedResponse(mp);
        TrayUpdate(mp);
        float x = WalkToTarget.x - p.x;
        if (Mathf.Abs(x) > 0.5f)
        {
            HController.Right = x > 0;
            HController.Left = x < 0;
        } else 
            HController.Remote = false;
        if (secondsRemaining > 0)
        {
            Vector2 psp = cam.ViewportToWorldPoint(new Vector3(0.05f, 0.95f, 10));
            TimerPS.transform.position = psp.ToVector3();
            TimerPS.startLifetime = (secondsRemaining / totalSeconds) * 15;
            secondsRemaining -= Time.deltaTime;

            if (secondsRemaining < 0)
            {
                TimerPS.enableEmission = false;
                TOScreen = Instantiate(TimeOver) as GameObject;
                TOScreen.transform.parent = this.transform;
                TOScreen.name = "Time Over Screen";
            }
        } else
        {
            TOScreen.transform.position = cam.ScreenToWorldPoint(new Vector3(Screen.width * 0.5f, Screen.height * 0.5f, 10));
        }
    }

    void TrayUpdate(Vector2 mp)
    {
        BoxCollider2D col = tray.obj.GetComponent<BoxCollider2D>();
        mouseInTray = col.OverlapPoint(mp);
        tray.Animate(cam, mouseInTray);
        for (int i = 0; i < items.Length; i++)
        {
            if(items[i].IsMouseOver(mp))
                items[i].Glow();
            else
                items[i].Mute();
            items [i].position = tray.slots [i];
        }
    }
    void ClickedResponse(Vector2 mp)
    {
        if (mouseInTray)
        {
            if(cursor.ActionID == -1)
            for (int i = 0; i < items.Length; i++)
                if(items[i].IsMouseOver(mp))
                    cursor.setCursor(items[i].obj, i);
        } else
        {
            switch(cursor.ActionID)
            {
                case -1:
                    // algorithm to find walkable point?
                    HController.Remote = true;
                    WalkToTarget = mp;
                    break;
                case 0:
                    goto default;
                case 1:
                    goto default;
                case 2:
                    goto default;
                case 3:
                    goto default;
                case 4:
                    goto default;
                case 5:

                    goto default;
                default:
                    cursor.setCursor(MouseIcon, -1);
                    break;
            }
        }
    }
}

public class MenuTray
{
    public GameObject obj;
    public Vector2 position;
    public Vector2[] slots = new Vector2[6];
    float animVal;

    public MenuTray(Transform parent, GameObject g, Vector3 size)
    {
        obj = g;
        BoxCollider2D col = obj.GetComponent<BoxCollider2D>();
        obj.transform.localScale = size;
        col.center = new Vector2(0, -0.25f);
        obj.name = "Tray";
        obj.transform.parent = parent;
        for (int i = 0; i < slots.Length; i++)
            slots [i] = new Vector2();
    }

    public void Animate(Camera cam, bool mouseOver)
    {
        BoxCollider2D col = obj.GetComponent<BoxCollider2D>();

        if (mouseOver)
            animVal = Mathf.Min(0.05f, animVal + Time.unscaledDeltaTime);
        else
            animVal = Mathf.Max(-0.15f, animVal - Time.unscaledDeltaTime);
        Vector2 p = cam.ScreenToWorldPoint(new Vector3(0.5f * Screen.width, animVal * Screen.height, 10));
        position = new Vector2(p.x, p.y + 0.5f);
        obj.transform.position = p;
        col.size = new Vector2(Screen.width * 0.1f, col.size.y);
        for (int i = 0; i < slots.Length; i++)
        {
            Vector2 v = cam.ViewportToWorldPoint(new Vector3((i + 1) * ((float)1 / (slots.Length + 1)), 0, 10));
            slots [i] = new Vector2(v.x, position.y);
        }
    }
}