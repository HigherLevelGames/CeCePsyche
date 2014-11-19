using UnityEngine;
using System.Collections;

public class PointNClickController : MonoBehaviour
{
    public GameObject ConditionableCharacter;
    public GameObject[] InventoryObjects;
    public GameObject MouseIcon;
    public GameObject Tray;
    public AudioClip BeepBoop;
    public AudioClip Bing;
    MovementController controller;
    Camera cam;
    PNCMenuTray tray;
    PNCItem[] items;
    PNCItem cursor;
    Vector2 WalkToTarget;

    void Start()
    {
        Screen.showCursor = true;
        controller = ConditionableCharacter.GetComponent<MovementController>();
        WalkToTarget = ConditionableCharacter.transform.position;
        cam = this.GetComponentInParent<Camera>();
        Vector3 s = cam.ScreenToWorldPoint(new Vector3(Screen.width * 0.9f, Screen.height * 0.8f, 20));
        tray = new PNCMenuTray(this.transform, Instantiate(Tray) as GameObject, s);
        //GameObject m = Instantiate(MouseIcon) as GameObject;
        //cursor = new PNCItem(m);
        items = new PNCItem[InventoryObjects.Length];
        for (int i = 0; i < InventoryObjects.Length; i++)
        {
            items [i] = new PNCItem(Instantiate(InventoryObjects [i]) as GameObject, (ItemActions)i);
            items [i].obj.transform.parent = this.transform;
            items [i].obj.name = "Slot" + i.ToString();
            items [i].SetActive(true);
        }
    }

    bool mouseInTray = false;

    void Update()
    {
        Vector2 mp = cam.ScreenToWorldPoint(Input.mousePosition + new Vector3(0, 0, 10));
        Vector2 p = ConditionableCharacter.transform.position;
        //cursor.position = mp;
        if (Input.GetMouseButtonDown(0))
            ClickedResponse(mp);
        TrayUpdate(mp);
        float x = WalkToTarget.x - p.x;
        if (Mathf.Abs(x) > 0.5f)
        {
            controller.Right = x > 0;
            controller.Left = x < 0;
        } else
            controller.Right = controller.Left = false;
    }

    void TrayUpdate(Vector2 mp)
    {
        BoxCollider2D col = tray.obj.GetComponent<BoxCollider2D>();
        mouseInTray = col.OverlapPoint(mp);
        tray.Animate(cam, mouseInTray);
        for (int i = 0; i < items.Length; i++)
        {
            if (items [i].IsMouseOver(mp))
                items [i].Glow();
            else
                items [i].Mute();
            items [i].position = tray.slots [i];
            if (items [i].Fired)
                FireItemAction(i);
        }
    }

    void ClickedResponse(Vector2 mp)
    {
        if (mouseInTray)
        {
            for (int i = 0; i < items.Length; i++)
            {
                if (items [i].IsMouseOver(mp))
                {
                    Animator ani = items [i].obj.GetComponentInChildren<Animator>();
                    ani.SetTrigger("PlayAnim");
                }
            }
        } else
        {
            WalkToTarget = mp;
        }
    }

    public void FireItemAction(int id)
    {
        PNCItem item = items [id];
        PerformItemAction(id, 0, item.ActionID);
        item.Fired = false;
    }

    void PerformItemAction(int caller_id, int target_id, ItemActions action)
    {
        switch (action)
        {
            case ItemActions.TuningFork:
                // access ConditionableCharacter
                // perform tuning fork on the character
                //      if conditioned, respond by salivating
                //      if not, respond by awareness of noise.

                break;
            case ItemActions.Dynamite:
                break;
            case ItemActions.DogBone:
                break;
        }
    }
}

public enum ItemActions
{
    Nothing = -1,
    TuningFork = 0, // plays a note and gets the dog's attention
    Dynamite = 1,
    DogBone = 2
}