using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum PMType
{
    Player,
    Neutral,
    Unconditioned,
    Response
}

public static class PsyData
{
    public static Transform parent;
    public static GameObject menuhex;
}

public class PsyModeController : MonoBehaviour
{
    enum MenuStage
    {
        Player,
        Target,
        Environment,
        Apply
    }
    public GameObject Player;
    public GameObject MenuHex;
    public GameObject[] Others;
    public GameObject[] Neutrals;
    public GameObject[] Environmentals;
    public GameObject[] Responses;
    float localScalar = 0;

    float antiScalar { get { return 1.0f - localScalar; } }

    Color c = new Color(0, 0, 1.0f, 0.6f);
    List<HexItem> hexItems = new List<HexItem>();
    PsyMenu nMenu, uMenu;
    PsyMenu[] tMenus;
    MenuStage  stage = MenuStage.Player;
    bool cooling;
    bool psyOn;
    int menuTarget = -1;
    int slotTarget = -1;
    Camera cam;
    SpriteRenderer bgrender;

    void Start()
    {
        PsyData.parent = this.transform;
        PsyData.menuhex = MenuHex;
        nMenu = new PsyMenu();
        uMenu = new PsyMenu();
        tMenus = new PsyMenu[Others.Length];
        for (int i = 0; i < tMenus.Length; i++)
            tMenus [i] = new PsyMenu();

        hexItems.Add(new HexItem(Neutrals [0], PMType.Neutral));
        hexItems.Add(new HexItem(Environmentals [0], PMType.Unconditioned));
        hexItems.Add(new HexItem(Responses [0], PMType.Response));

        CloseMenu();
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

                if (Input.GetKeyDown(KeyCode.X))
                    Confirm();
                if (Input.GetKeyDown(KeyCode.A))
                    GetNextSlot();
                if (Input.GetKeyDown(KeyCode.D))
                    GetPrevSlot();
                SetPositions();
                for (int i = 0; i < hexItems.Count; i++)
                    hexItems [i].Scale(localScalar);
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
        nMenu.position = Player.transform.position.ToVector2();
        nMenu.RoundMenuAnimate();
        uMenu.position = cam.ScreenToWorldPoint(new Vector3(Screen.width * 0.03f, Screen.height * 0.93f, 10));
        uMenu.BoxMenuAnimate();
        for (int i = 0; i < Others.Length; i++)
            if (Others [i] && i != menuTarget)
            {
                tMenus [i].position = Others [i].transform.position.ToVector2();
                tMenus [i].SoloAnimate();
            }
        tMenus [menuTarget].position = Others [menuTarget].transform.position.ToVector2();
        tMenus [menuTarget].RoundMenuAnimate();
    }
    #endregion




    #region Tools
    void Cool()
    {
        cooling = true;
        uMenu.Deselect();
        nMenu.Deselect();
        for (int i = 0; i < tMenus.Length; i++)
            tMenus [i].Deselect();
    }

    void WarmMenu()
    {
        psyOn = true;
        cooling = false;
        menuTarget = -1;
        for (int i = 0; i < tMenus.Length; i++)
            if (isTargetOnScreen(i))
                menuTarget = i;
        if (menuTarget > -1)
        {
            for (int i = 0; i < hexItems.Count; i++)
                hexItems [i].SetActive(true);
            nMenu.Open();
            uMenu.Open();
            tMenus [menuTarget].Open();
            for (int i = 0; i < tMenus.Length; i++)
                tMenus [i].SetActive(true);
            SetPositions();
        }

    }
    
    void CloseMenu()
    {
        psyOn = false;
        cooling = false;
        localScalar = 0;
        for (int i = 0; i < hexItems.Count; i++)
            hexItems [i].SetActive(false);
        uMenu.Close();
        nMenu.Close();
        for (int i = 0; i < tMenus.Length; i++)
            tMenus [i].Close();
    }

    bool isTargetOnScreen(int i)
    {
        Vector2 p = cam.WorldToViewportPoint(new Vector3(tMenus [i].position.x, tMenus [i].position.y, 0));
        if (p.x > 0 && p.x < 1 && p.y > 0 && p.y < 1)
            return true;
        return false;           
    }

    void GetPrevSlot()
    {
        int n;
        switch (stage)
        {
            case MenuStage.Player:
                break;
            case MenuStage.Target:
                tMenus [menuTarget].Deselect();
                for (int i = menuTarget; i > -1; i--)
                    if (isTargetOnScreen(i))
                        menuTarget = i;
                break;
            case MenuStage.Environment:
                slotTarget = Mathf.Max(slotTarget - 1, 0);
                break;
            case MenuStage.Apply:
                break;
        }
    }
    
    void Confirm()
    {
        switch (stage)
        {
            case MenuStage.Player:
                stage = MenuStage.Target;
                break;
            case MenuStage.Target:
                break;
            case MenuStage.Environment:
                break;
            case MenuStage.Apply:
                break;
                
        }
    }

    void GetNextSlot()
    {
        int n;
        switch (stage)
        {
            case MenuStage.Player:
                break;
            case MenuStage.Target:
                tMenus [menuTarget].Deselect();
                for (int i = menuTarget; i < tMenus.Length; i++)
                    if (isTargetOnScreen(i))
                        menuTarget = i;
                break;
            case MenuStage.Environment:
                slotTarget = Mathf.Min(slotTarget + 1, uMenu.Slots.Length); 
                break;
            case MenuStage.Apply:
                break;
                        
        }
    }
    #endregion
    #region Debug
    void OnDrawGizmos()
    {
        /*
        if (psyOn)
        {
            for (int i = 0; i < nSlots.Length; i++)
                Gizmos.DrawIcon(nSlots [i].position.ToVector3(), "icon.tiff");
            for (int i = 0; i < uSlots.Length; i++)
                Gizmos.DrawIcon(uSlots [i].position.ToVector3(), "icon.tiff");
            for (int j = 0; j < targets.Length; j++)
                for (int i = 0; i < targets[j].Slots .Length; i++)
                    Gizmos.DrawIcon(targets [j].Slots [i].position.ToVector3(), "icon.tiff");
        }
        */
    }

    void OnGUI()
    {
        if (menuTarget < 0 && psyOn)
        {
            GUI.Label(new Rect(Screen.width * 0.5f, Screen.height * 0.5f, 100, 50), "No Targets Found.");
        }
    }
    #endregion
}






