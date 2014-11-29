using UnityEngine;
using System.Collections.Generic;

public class PointNClickGame : MonoBehaviour
{
    public GameObject ConditionableCharacter;
    public GameObject[] OtherCharacters;
    public GameObject[] InventoryObjects;
    public GameObject Tray;
    public string Prompt;
    public string Hint;
    public string Lose;
    public string Win;
    public bool WinConditionMet, ClickToMove;
    Conditionable condition;

    public Conditionable GetCondition
    {
        get { return condition; }
    }

    MovementController controller;
    Camera cam;
    PNCMenuTray tray;
    PNCItem[] items;
    Vector2 WalkToTarget;

    public void Activate()
    {
        Screen.showCursor = true;
        ConditionableCharacter.SetActive(true);
        for (int i = 0; i < OtherCharacters.Length; i++)
            OtherCharacters [i].SetActive(true);
    }

    public void Initialize()
    {
        controller = ConditionableCharacter.GetComponent<MovementController>();
        condition = ConditionableCharacter.GetComponent<Conditionable>();
        WalkToTarget = ConditionableCharacter.transform.position;
        cam = this.GetComponentInParent<Camera>();
        Vector3 s = cam.ScreenToWorldPoint(new Vector3(Screen.width * 0.9f, Screen.height * 0.8f, 20));
        tray = new PNCMenuTray(this.transform, Instantiate(Tray) as GameObject, s);
        items = new PNCItem[InventoryObjects.Length];
        items [0] = new PNCItem(Instantiate(InventoryObjects [0]) as GameObject, ItemActions.TuningFork);
        items [1] = new PNCItem(Instantiate(InventoryObjects [1]) as GameObject, ItemActions.StinkyPerfume);
        items [2] = new PNCItem(Instantiate(InventoryObjects [2]) as GameObject, ItemActions.ZapNectar);
        for (int i = 0; i < InventoryObjects.Length; i++)
        {
            items [i].obj.transform.parent = this.transform;
            items [i].obj.name = "Slot" + i.ToString();
            items [i].Active = true;
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
        if (condition.Consuming)
        {
            PerformItemAction(0, 0, (ItemActions)condition.ConditionedResponse);
            condition.Consuming = false;
        }
        float x = WalkToTarget.x - p.x;
        if (ClickToMove)
        {
            if (Mathf.Abs(x) > 1)
            {
                controller.Right = x > 0;
                controller.Left = x < 0;
            } else
                controller.Right = controller.Left = false;
        }
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
                FireMenuItemAction(i);
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
            if (ClickToMove)
                WalkToTarget = mp;
        }
    }

    public void FireMenuItemAction(int id)
    {
        PNCItem item = items [id];
        PerformItemAction(id, 0, item.ActionID);
        item.Fired = false;
    }

    public void PerformItemAction(int caller_id, int target_id, ItemActions action)
    {
        Animator anim = ConditionableCharacter.GetComponentInChildren<Animator>();
        PlaySound ps = ConditionableCharacter.GetComponentInChildren<PlaySound>();
        CheckWinCondition(action);
        int act = (int)action;
        if (condition.ConditionedStimulus > -1)
            action = (ItemActions)condition.ConditionedResponse;
        if (condition.CurrentEnjoyedBehavior > -1)
            condition.AttemptAversionWith(act);
        if (condition.TasteAvertedBehavior == act)
            return;
        switch (action)
        {
            case ItemActions.TuningFork:
                condition.AddNeutral(act);
                anim.SetTrigger("SetWaiting");
                ps.PlayAudioOnce();
                break;
            case ItemActions.Dynamite:
                break;
            case ItemActions.DogBone:
                condition.AddUnconditioned(act);
                anim.SetTrigger("SetHappy");
                break;
            case ItemActions.StinkyPerfume:
                condition.AddUnconditioned(act);
                anim.SetTrigger("SetDisgusted");
                break;
            case ItemActions.Squirrel:
                condition.CurrentEnjoyedBehavior = act;
                condition.AddNeutral(act);
                if(condition.CurrentEnjoyedBehavior == act)anim.SetTrigger("SetChase");
                break;
            case ItemActions.ZapNectar:
                condition.AddUnconditioned(act);
                anim.SetTrigger("SetZapped");
                break;
        }
    }

    public virtual void CheckWinCondition(ItemActions action)
    {
    }

    public void Reset()
    {
        WinConditionMet = false;
        controller.Right = controller.Left = false;
        condition.Reset();
    }

    public void Cleanup()
    {
        Screen.showCursor = false;
        Reset();
        ConditionableCharacter.SetActive(false);
        for (int i = 0; i < OtherCharacters.Length; i++)
            OtherCharacters [i].SetActive(false);
    }
}

public enum ItemActions
{
    Nothing = -1,
    TuningFork = 0, // plays a note and gets the dog's attention
    Dynamite = 1,
    DogBone = 2,
    StinkyPerfume = 3,
    ZapNectar = 4,
    Squirrel = 5
}