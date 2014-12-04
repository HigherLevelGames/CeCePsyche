using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HexModeController : MonoBehaviour
{
    enum MenuStage
    {
        Init,
        Inventory,
        Target,
        Apply
    }
    public GameObject MenuHex;
    GameObject[] inventory;
    float localScalar = 0;

    float antiScalar { get { return 1.0f - localScalar; } }

    Color c = new Color(0, 0, 1.0f, 0.6f);
    PsyMenu nMenu;
    PsyMenu[] tMenus;
    MenuStage  stage = MenuStage.Init;
    bool cooling;
    bool psyOn;
    int menuTarget = -1;
    int slotTarget = 0;
    Camera cam;
    SpriteRenderer bgrender;

    void Start()
    {
        nMenu = new PsyMenu(3);
        nMenu.hex.name = "Player Menu";

        CloseMenu();
        inventory = InventoryManager.data.inventories [0].GetItemObjects(); 
        cam = this.GetComponentInParent<Camera>();
        bgrender = this.GetComponentInChildren<SpriteRenderer>();
        bgrender.color = new Color(c.r, c.g, c.b, c.a * antiScalar);
    }

    void Update()
    {
        if (psyOn)
        {
            if (Input.GetKeyDown(KeyCode.Z))
                Cool();
            if (cooling)
            {
                localScalar -= Time.unscaledDeltaTime * 2;
                if (localScalar < 0.0f)
                    CloseMenu();
            } else if (localScalar < 1.0f)
                localScalar = Mathf.Min(1, localScalar + Time.unscaledDeltaTime * 2);
            if (menuTarget > -1)
            {
                if (!cooling)
                {
                    if (Input.GetKeyDown(KeyCode.X))
                        Confirm();
                    if (Input.GetKeyDown(KeyCode.A))
                        GetPrevSlot();
                    if (Input.GetKeyDown(KeyCode.D))
                        GetNextSlot();
                }
                ShowSelection();
                SetPositions();
            }
        } else
        {
            if (Input.GetKeyDown(KeyCode.Z))
                WarmMenu();
        }


        Time.timeScale = antiScalar;
        bgrender.color = new Color(c.r, c.g, c.b, c.a * localScalar);
    }
    #region Update Functions
    void SetPositions()
    {
        //nMenu.position = Player.transform.position.ToVector2();
        nMenu.RoundMenuAnimate();
        //for (int i = 0; i < Others.Length; i++)
        // if (Others [i])
        // {
        //      tMenus [i].position = Others [i].transform.position.ToVector2();
        //      tMenus [i].RoundMenuAnimate();
        //  }
    }
    #endregion


    void ShowSelection()
    {
        tMenus [menuTarget].Glow();
        if (slotTarget < 0)
            return;
        switch (stage)
        {
            case MenuStage.Inventory:
                nMenu.Slots [slotTarget].Glow();
                break;
            case MenuStage.Apply:
                break;
        }
    }

    #region Tools
    void Cool()
    {
        slotTarget = 0;
        cooling = true;
        nMenu.Cool();
        for (int i = 0; i < tMenus.Length; i++)
            tMenus [i].Cool();
    }

    void WarmMenu()
    {
        psyOn = true;
        cooling = false;
        menuTarget = -1;
        slotTarget = 0;
        //for (int i = 0; i < Others.Length; i++)
        //    tMenus [i].position = Others [i].transform.position.ToVector2();
        for (int i = 0; i < tMenus.Length; i++)
            if (isTargetOnScreen(i))
            {
                menuTarget = i;
                break;
            }
        if (menuTarget > -1)
        {
            nMenu.Open();
            nMenu.Select();
            for (int i = 0; i < tMenus.Length; i++)
            {
                tMenus [i].SetActive(true);
                if (menuTarget != i)
                    tMenus [i].Deselect();
            }
            tMenus [menuTarget].Open();
            SetPositions();
        }
        stage = MenuStage.Init;
    }
    
    void CloseMenu()
    {
        psyOn = false;
        cooling = false;
        localScalar = 0;
        nMenu.Close();
        for (int i = 0; i < tMenus.Length; i++)
            tMenus [i].Close();
    }

    void GetPrevSlot()
    {
        switch (stage)
        {
            case MenuStage.Inventory:
                nMenu.Slots [slotTarget].Deselect();
                slotTarget = (slotTarget + 1) % nMenu.Slots.Length; 
                nMenu.Slots [slotTarget].Select();
                break;
            case MenuStage.Target:
                int t = -1;
                float cdist = 100;
                for (int i = 0; i < tMenus.Length; i++)
                    if (i != menuTarget && isTargetOnScreen(i))
                    {
                        float d = tMenus [i].position.x - tMenus [menuTarget].position.x;
                        if (d < 0 && d < cdist)
                        {
                            cdist = d;
                            t = i;
                        }
                    }
                if (t > -1)
                {
                    tMenus [menuTarget].Deselect();
                    tMenus [t].Select();
                    menuTarget = t;
                }
                break;
            case MenuStage.Apply:
                break;
        }
    }
    
    void Confirm()
    {
        switch (stage)
        {
            case MenuStage.Inventory:
                stage = MenuStage.Target;
                nMenu.Slots [slotTarget].Select();
                nMenu.SortTo(10002);
                slotTarget = 0;
                break;
            case MenuStage.Target:
                stage = MenuStage.Apply;
                slotTarget = 0;
                break;
            case MenuStage.Apply:
                break;
        }
    }

    void GetNextSlot()
    {
        switch (stage)
        {
            case MenuStage.Inventory:
                nMenu.Slots [slotTarget].Deselect();
                slotTarget = (slotTarget + nMenu.Slots.Length - 1) % nMenu.Slots.Length; 
                nMenu.Slots [slotTarget].Select();
                break;
            case MenuStage.Target:
                int t = -1;
                float cdist = 100;
                for (int i = 0; i < tMenus.Length; i++)
                    if (i != menuTarget && isTargetOnScreen(i))
                    {
                        float d = tMenus [i].position.x - tMenus [menuTarget].position.x;
                        if (d > 0 && d < cdist)
                        {
                            cdist = d;
                            t = i;
                        }
                    }
                if (t > -1)
                {
                    tMenus [menuTarget].Deselect();
                    tMenus [t].Select();
                    menuTarget = t;
                }
                break;
            case MenuStage.Apply:
                break;       
        }
    }

    bool isTargetOnScreen(int i)
    {
        Vector2 p = cam.WorldToViewportPoint(new Vector3(tMenus [i].position.x, tMenus [i].position.y, 0));
        if (p.x > 0 && p.x < 1 && p.y > 0 && p.y < 1)
            return true;
        return false;           
    }
    #endregion
    #region Debug
    void OnGUI()
    {
        if (menuTarget < 0 && psyOn)
            GUI.Label(new Rect(Screen.width * 0.5f, Screen.height * 0.5f, 100, 50), "No Targets Found.");
    }
    #endregion
}






