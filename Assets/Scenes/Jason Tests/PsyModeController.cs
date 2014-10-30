using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum PMType
{
    Neutral,
    Unconditioned,
    Response
}

public class PsyModeController : MonoBehaviour
{
    public GameObject Player;
    public GameObject[] Others;
    public GameObject[] Neutrals;
    public GameObject[] Environmentals;
    public GameObject[] Responses;
    SpriteRenderer bgrender;
    Camera cam;
    float localScalar = 0;

    float antiScalar { get { return 1.0f - localScalar; } }

    Color c = new Color(0, 0, 1.0f, 0.7f);
    PsyModeSlot[] menuSlot = new PsyModeSlot[21];
    ResponseSlot[] targets;
    List<PsyModeObject> psyObjects = new List<PsyModeObject>();
    bool cooling;
    bool psyOn;

    void Start()
    {

        for (int i = 0; i < menuSlot.Length; i++)
            menuSlot [i] = new PsyModeSlot();
        AddPsyObject(Instantiate(Neutrals [0]) as GameObject, this.transform, PMType.Neutral, 0);
        AddPsyObject(Instantiate(Environmentals [0]) as GameObject, this.transform, PMType.Unconditioned, 12);
        AddPsyObject(Instantiate(Responses [0]) as GameObject, this.transform, PMType.Response, 20);
        cam = this.GetComponentInParent<Camera>();
        bgrender = this.GetComponentInChildren<SpriteRenderer>();
        bgrender.color = new Color(c.r, c.g, c.b, c.a * antiScalar);
    }

    ResponseSlot[] FindPossibleTargets()
    {
        List<ResponseSlot> slots = new List<ResponseSlot>();
        for (int i = 0; i < Others.Length; i++)
        {
            Vector2 p = cam.WorldToViewportPoint(new Vector3(Others [i].transform.position.x, Others [i].transform.position.y, 0));
            if (p.x > 0 && p.x < 1 && p.y > 0 && p.y < 1)
            {
                ResponseSlot rs = new ResponseSlot();
                rs.position = Others [i].transform.position.ToVector2();
                // Find responses to draw
                rs.ItemID = 0;
                rTarget = 0;
                slots.Add(rs);
            }
        }
        return slots.ToArray();
    }

    void Update()
    {
        if (psyOn)
        {
            if (Input.GetKeyDown(KeyCode.Z))
                CloseMenu();
            if (cooling)
            {
                localScalar -= Time.unscaledDeltaTime * 2;
                if (localScalar < 0.0f)
                {
                    psyOn = false;
                    cooling = false;
                    for (int i = 0; i < psyObjects.Count; i++)
                        psyObjects [i].SetActive(false);
                    localScalar = 0;
                }
            } else if (localScalar < 1.0f)
                localScalar = Mathf.Min(1, localScalar + Time.unscaledDeltaTime * 2);
            if (rTarget > -1)
            {
                Vector2 op = targets [rTarget].position;
                Vector2 pp = Player.transform.position.ToVector2();
                Vector2 ep = cam.ScreenToWorldPoint(new Vector3(Screen.width * 0.02f, Screen.height * 0.95f, 10));
                if (Input.GetKeyDown(KeyCode.X))
                    Confirm();
                if (Input.GetKeyDown(KeyCode.A))
                    GetNextSlot();
                if (Input.GetKeyDown(KeyCode.D))
                    GetPrevSlot();
                SetPositions(pp, op, ep, 0.8f);
                for (int i = 0; i < psyObjects.Count; i++)
                    psyObjects [i].Update(localScalar);
            }
        } else
        {
            if (Input.GetKeyDown(KeyCode.Z))
                WarmMenu();
        }


        Time.timeScale = antiScalar;
        bgrender.color = new Color(c.r, c.g, c.b, c.a * localScalar);
    }

    /// <summary>
    ///  BOOKMARKED SPOT
    /// </summary>
    void DrawResponseSlots()
    {
        for (int i = 0; i < targets.Length; i++)
        {
            for (int j = 0; j < targets[i].Slots.Length; j++)
            {

            }
        }
    }
    enum MenuStage
    {
        Target,
        Environment,
        Apply
    }
    MenuStage stage = MenuStage.Target;

    void Confirm()
    {
        switch (stage)
        {
            case MenuStage.Target:
                nTarget = 12;
                break;
            case MenuStage.Environment:
                break;
            case MenuStage.Apply:
                break;
            
        }
    }

    int rTarget;
    int uTarget;
    int nTarget;

    void GetNextSlot()
    {
        int n;
        switch (stage)
        {
            case MenuStage.Target:
                rTarget = Mathf.Min(rTarget + 1, Others.Length - 1);
                break;
            case MenuStage.Environment:
                n = nTarget + 1;
                if (menuSlot [n].ItemID > -1 && n < 20)
                    nTarget++;
                break;
            case MenuStage.Apply:
                break;
                
        }
    }

    void GetPrevSlot()
    {
        int n;
        switch (stage)
        {
            case MenuStage.Target:
                rTarget = Mathf.Max(rTarget - 1, 0);
                break;
            case MenuStage.Environment:
                n = nTarget - 1;
                if (menuSlot [n].ItemID > -1 && n > 5)
                    nTarget = n;
                break;
            case MenuStage.Apply:
                break;
                
        }
    }

    void AddPsyObject(GameObject g, Transform parent, PMType t, int atID)
    {
        g.transform.parent = parent;
        psyObjects.Add(new PsyModeObject(g, t));
        menuSlot [atID].ItemID = psyObjects.Count - 1;
    }

    void SetPositions(Vector2 pp, Vector2 op, Vector2 ep, float rad)
    {
        float srad = rad * localScalar;
        float bonusRotation = Mathf.PI * 2 * localScalar;
        for (int i = 0; i < menuSlot.Length; i++)
        {
            
            float r;
            float x; 
            float y; 
            if (i < 6)
            {
                r = ((float)i / 6) * Mathf.PI * 2 * localScalar + bonusRotation;
                x = srad * (Mathf.Cos(r) - Mathf.Sin(r));
                y = srad * (Mathf.Sin(r) + Mathf.Cos(r));
                menuSlot [i].position = new Vector2(pp.x + x, pp.y + y);
            } else if (i < 12)
            {
                r = ((float)i / 6) * Mathf.PI * 2 * localScalar + bonusRotation;
                x = srad * (Mathf.Cos(r) - Mathf.Sin(r));
                y = srad * (Mathf.Sin(r) + Mathf.Cos(r));
                menuSlot [i].position = new Vector2(op.x + x, op.y + y);
            } else if (i < menuSlot.Length - 1)
            {
                x = i - 11;
                r = Mathf.Cos(x * Mathf.PI / (menuSlot.Length - 12));
                y = r - r * localScalar;
                menuSlot [i].position = new Vector2(ep.x + x, ep.y + y);
            } else
                menuSlot [i].position = op;
            if (menuSlot [i].ItemID > -1)
                psyObjects [menuSlot [i].ItemID].Target = menuSlot [i].position;
        }
    }

    void WarmMenu()
    {

        stage = MenuStage.Target;
        psyOn = true;
        cooling = false;
        for (int i = 0; i < psyObjects.Count; i++)
            psyObjects [i].SetActive(true);
        targets = FindPossibleTargets();
        if (targets.Length < 1)
            rTarget = -1;
    }
    
    void CloseMenu()
    {
        cooling = true;
    }

    void OnGUI()
    {
        if (rTarget < 0 && psyOn)
        {
            GUI.Label(new Rect(Screen.width * 0.5f, Screen.height * 0.5f, 100, 50), "No Targets Found.");
        }
    }
    #region Debug
    void OnDrawGizmos()
    {
        if (psyOn)
            for (int i = 0; i < menuSlot.Length; i++)
                Gizmos.DrawIcon(menuSlot [i].position.ToVector3(), "icon.tiff");
    }
    #endregion
}

public class PsyModeSlot
{
    public Vector2 position;
    public int ItemID = -1;
}

public class ResponseSlot : PsyModeSlot
{
    public PsyModeSlot[] Slots = new PsyModeSlot[6];

    public ResponseSlot()
    {
        for (int i = 0; i < Slots.Length; i++)
            Slots [i] = new PsyModeSlot();
    }
}

public class PsyModeObject
{
    public PMType Type;
    float lerpScalar = 1.0f;
    bool reverse;
    float glowValue;
    GameObject g;
    SpriteRenderer spr;
    public Vector2 Target;
    Vector2 old;

    public PsyModeObject(GameObject g, PMType t)
    {
        this.g = g;
        this.spr = g.GetComponent<SpriteRenderer>();
        this.Type = t;
        old = Target = g.transform.position;
    }

    public void Update(float scalar)
    {
        if (lerpScalar < 1.0f)
        {
            position = Vector2.Lerp(old, Target, lerpScalar);
            lerpScalar += Time.unscaledDeltaTime;
        } else
        {
            lerpScalar = 1.0f;
            position = Target;
        }
        g.transform.localScale = Vector3.one * scalar;
    }

    public void Glow()
    {
        if (glowValue > 1)
            reverse = true;
        if (glowValue < 0)
            reverse = false;
        if (reverse)
            glowValue -= Time.unscaledDeltaTime;
        else
            glowValue += Time.unscaledDeltaTime;
        spr.material.SetFloat("_GlowRate", glowValue);
    }

    public void SetActive(bool val)
    {
        g.SetActive(val);
    }

    public void StartMove()
    {
        old = position;
        lerpScalar = 0.0f;
    }

    public Vector2 position
    {
        get { return g.transform.position.ToVector2(); }
        set { g.transform.position = value; }
    }
}
