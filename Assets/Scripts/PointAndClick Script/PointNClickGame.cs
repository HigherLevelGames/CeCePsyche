using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class PointNClickGame : MonoBehaviour
{
    [HideInInspector]
    public string
        Prompt, Hint, Lose, Win;
    [HideInInspector]
    public bool
        WinConditionMet;
    public GameObject ConditionableCharacter;
    public GameObject[] OtherCharacters;
    public bool ClickToMove = false;
    Conditionable condition;
    Inventory inventory;

    public Conditionable GetCondition
    {
        get { return condition; }
    }

    MovementController controller;
    Camera cam;
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
        inventory = InventoryManager.data.inventories [0];
        controller = ConditionableCharacter.GetComponent<MovementController>();
        condition = ConditionableCharacter.GetComponent<Conditionable>();
        WalkToTarget = ConditionableCharacter.transform.position;
        cam = this.GetComponentInParent<Camera>();
    }

    void Update()
    {
        if (condition.Consuming)
        {
            PerformItemAction(0, 0, (ItemActions)condition.ConditionedResponse);
            condition.Consuming = false;
        }

        if (ClickToMove && controller)
        {
            Vector2 mp = cam.ScreenToWorldPoint(Input.mousePosition + new Vector3(0, 0, 10));
            Vector2 p = ConditionableCharacter.transform.position;
            if (Input.GetMouseButtonDown(0))
                WalkToTarget = mp;
            float x = WalkToTarget.x - p.x;
            if (Mathf.Abs(x) > 1)
            {
                controller.Right = x > 0;
                controller.Left = x < 0;
            } else
                controller.Right = controller.Left = false;
        }
    }

    public void FireMenuItemAction(int id)
    {
        if (id < inventory.Items.Count)
            PerformItemAction(id, 0, inventory.Items [id]);
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
            case ItemActions.SqueakyToy:
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
                if (condition.CurrentEnjoyedBehavior == act)
                    anim.SetTrigger("SetChase");
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

    public virtual void ClickFunction(int id)
    {
    }

    public void Reset()
    {
        WinConditionMet = false;
        if (controller)
        {
            controller.Right = controller.Left = false;
            condition.Reset();
        }
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