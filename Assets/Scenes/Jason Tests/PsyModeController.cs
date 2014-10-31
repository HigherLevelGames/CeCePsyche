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
    enum MenuStage
    {
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
    static Transform parent;
    SpriteRenderer bgrender;
    Camera cam;
    float localScalar = 0;

    float antiScalar { get { return 1.0f - localScalar; } }

    Color c = new Color(0, 0, 1.0f, 0.7f);
    PsyModeSlot[] nSlots = new PsyModeSlot[6];
    PsyModeSlot[] uSlots = new PsyModeSlot[8];
    ResponseSlot[] targets;
    List<PsyModeObject> psyObjects = new List<PsyModeObject>();
    bool cooling;
    bool psyOn;
    bool glowReverse;
    float glowval;
    static GameObject menuHex;
    static GameObject[] neutrals; 
    void Start()
    {
        parent = this.transform;
        menuHex = MenuHex; neutrals = Neutrals;
        targets = new ResponseSlot[Others.Length];
        for (int i = 0; i < Others.Length; i++)
        {
            targets [i] = new ResponseSlot();
            targets [i].position = Others [i].transform.position;
        }
        for (int i = 0; i < nSlots.Length; i++)
            nSlots [i] = new PsyModeSlot();
        for (int i = 0; i < uSlots.Length; i++)
            uSlots [i] = new PsyModeSlot();
        psyObjects.Add(new PsyModeObject(PMType.Neutral));
        psyObjects.Add(new PsyModeObject(PMType.Unconditioned));
        psyObjects.Add(new PsyModeObject(PMType.Response));
        SetSlot(0, 0);
        SetSlot(1, 0);
        SetSlot(2, 0);
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
                cooling = true;
            if (cooling)
            {
                localScalar -= Time.unscaledDeltaTime * 2;
                if (localScalar < 0.0f)
                {
                    psyOn = false;
                    cooling = false;
                    CloseMenu();

                    localScalar = 0;
                }
            } else if (localScalar < 1.0f)
                localScalar = Mathf.Min(1, localScalar + Time.unscaledDeltaTime * 2);
            if (rTarget > -1)
            {

                if (Input.GetKeyDown(KeyCode.X))
                    Confirm();
                if (Input.GetKeyDown(KeyCode.A))
                    GetNextSlot();
                if (Input.GetKeyDown(KeyCode.D))
                    GetPrevSlot();
                SetPositions();
                GlowMenu();
                for (int i = 0; i < psyObjects.Count; i++)
                {
                    psyObjects [i].Update(localScalar);
                }
                cam.transform.position += (targets [rTarget].position - cam.transform.position.ToVector2()).ToVector3() * Time.unscaledDeltaTime;
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
        Vector2 op = targets [rTarget].position;
        Vector2 pp = Player.transform.position.ToVector2();
        Vector2 ep = cam.ScreenToWorldPoint(new Vector3(Screen.width * 0.02f, Screen.height * 0.95f, 10));
        Vector2 p = new Vector2();
        float srad = 0.8f * localScalar;
        float bonusRotation = Mathf.PI * 2 * localScalar;
        float r;
        float x; 
        float y; 
        for (int i = 0; i < nSlots.Length; i++)
        {
            r = ((float)i / 6) * Mathf.PI * 2 * localScalar + bonusRotation;
            x = srad * (Mathf.Cos(r) - Mathf.Sin(r));
            y = srad * (Mathf.Sin(r) + Mathf.Cos(r));
            nSlots [i].position = new Vector2(pp.x + x, pp.y + y);
            if (nSlots [i].ItemID > -1)
                psyObjects [nSlots [i].ItemID].Target = nSlots [i].position;
        }
        for (int i = 0; i < uSlots.Length; i++)
        {
            x = (float)i + 1;
            r = Mathf.Cos(x * Mathf.PI / uSlots.Length);
            y = r - r * localScalar;
            uSlots [i].position = new Vector2(ep.x + x, ep.y + y);
            if (uSlots [i].ItemID > -1)
                psyObjects [uSlots [i].ItemID].Target = uSlots [i].position;
        }
        for (int j = 0; j < targets.Length; j++)
        {
            p = targets [j].position;
            for (int i = 0; i < targets[j].Slots.Length; i++)
            {
                r = ((float)i / targets [j].Slots.Length) * Mathf.PI * 2 * localScalar + bonusRotation;
                
                x = srad * (Mathf.Cos(r) - Mathf.Sin(r));
                y = srad * (Mathf.Sin(r) + Mathf.Cos(r));
                targets [j].Slots [i].position = new Vector2(p.x + x, p.y + y);
            }
            if (targets [j].ItemID > -1)
                psyObjects [targets [j].ItemID].Target = targets [j].position;
        } 
    }
    #endregion

    void GlowMenu()
    {
        glowval += Time.unscaledDeltaTime * 5;
        if (glowval > Mathf.PI * 2.0f)
            glowval -= Mathf.PI * 2.0f;
        float n = Mathf.Sin(glowval) * 0.5f;
        Debug.Log(n);
        switch (stage)
        {
            case MenuStage.Target:
                targets[rTarget].hex.renderer.material.SetFloat("_GlowRate", n);
                for(int i = 0; i < targets[rTarget].Slots.Length; i++)
                {
                    targets[rTarget].Slots[i].hex.renderer.material.SetFloat("_GlowRate", n);
                }
                break;
        }
    }

    void OnGUI()
    {
        if (rTarget < 0 && psyOn)
        {
            GUI.Label(new Rect(Screen.width * 0.5f, Screen.height * 0.5f, 100, 50), "No Targets Found.");
        }
    }
    #region Tools
    void SetSlot(int psyID, int slotID)
    {
        switch (psyObjects [psyID].Type)
        {
            case PMType.Neutral:
                nSlots [slotID].ItemID = psyID;
                break;
            case PMType.Response:
                targets [slotID].ItemID = psyID;
                break;
            case PMType.Unconditioned:
                uSlots [slotID].ItemID = psyID;
                break;
        }
    }

    void SetSlot(int psyID, int slotID, int responseID)
    {
        targets [responseID].Slots [slotID].ItemID = psyID;
    }
    
    void WarmMenu()
    {
        
        stage = MenuStage.Target;
        psyOn = true;
        cooling = false;
        rTarget = -1;
        for (int i = 0; i < targets.Length; i++)
            if (isTargetOnScreen(i))
                rTarget = i;
        if (rTarget > -1)
        {
            for (int i = 0; i < psyObjects.Count; i++)
                psyObjects [i].SetActive(true);
            for(int i = 0; i < nSlots.Length; i++)
                nSlots[i].SetActive(true);
            for(int i = 0; i < uSlots.Length; i++)
                uSlots[i].SetActive(true);
            for(int i = 0; i < targets.Length; i++)
                targets[i].SetActive(true);
        }
        
    }
    
    void CloseMenu()
    {
        for (int i = 0; i < psyObjects.Count; i++)
            psyObjects [i].SetActive(false);
        for(int i = 0; i < nSlots.Length; i++)
            nSlots[i].SetActive(false);
        for(int i = 0; i < uSlots.Length; i++)
            uSlots[i].SetActive(false);
        for(int i = 0; i < targets.Length; i++)
            targets[i].SetActive(false);
    }

    bool isTargetOnScreen(int i)
    {
        Vector2 p = cam.WorldToViewportPoint(new Vector3(targets [i].position.x, targets [i].position.y, 0));
        if (p.x > 0 && p.x < 1 && p.y > 0 && p.y < 1)
            return true;
        return false;           
    }

    void GetPrevSlot()
    {
        int n;
        switch (stage)
        {
            case MenuStage.Target:
                targets[rTarget].hex.renderer.material.SetFloat("_GlowRate", -0.5f);
                for(int i = 0; i < targets[rTarget].Slots.Length; i++)
                    targets[rTarget].Slots[i].hex.renderer.material.SetFloat("_GlowRate", -0.5f);
                for (int i = rTarget; i > -1; i--)
                    if (isTargetOnScreen(i))
                        rTarget = i;
                break;
            case MenuStage.Environment:
                nTarget = Mathf.Max(nTarget - 1, 0);
                break;
            case MenuStage.Apply:
                break;
        }
    }

    int rTarget;
    int uTarget;
    int nTarget;
    MenuStage stage = MenuStage.Target;
    
    void Confirm()
    {
        switch (stage)
        {
            case MenuStage.Target:
                for (int i = rTarget; i > -1; i--)
                    if (isTargetOnScreen(i))
                        rTarget = i;
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
            case MenuStage.Target:
                targets[rTarget].hex.renderer.material.SetFloat("_GlowRate", -0.5f);
                for(int i = 0; i < targets[rTarget].Slots.Length; i++)
                    targets[rTarget].Slots[i].hex.renderer.material.SetFloat("_GlowRate", -0.5f);
                for (int i = rTarget; i < targets.Length; i++)
                    if (isTargetOnScreen(i))
                        rTarget = i;
                break;
            case MenuStage.Environment:
                uTarget = Mathf.Min(uTarget + 1, uSlots.Length); 
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
    public class PsyModeSlot
    {
        public GameObject hex;
        
        public Vector2 position
        {
            get { return p; }
            set { p = value; hex.transform.position = value; }
        }
        Vector2 p;
        public int ItemID = -1;
        public void SetActive(bool b)
        {
            this.hex.SetActive(b);
        }
        public PsyModeSlot()
        {
            hex = Instantiate(menuHex) as GameObject;
            hex.transform.parent = parent;
        }
    }
    
    public class ResponseSlot : PsyModeSlot
    {
        public PsyModeSlot[] Slots = new PsyModeSlot[6];
        public void SetActive(bool b)
        {
            this.hex.SetActive(b);
            for (int i = 0; i < Slots.Length; i++)
                Slots [i].SetActive(b);
        }
        public ResponseSlot()
        {
            hex = Instantiate(menuHex) as GameObject;
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
        
        public PsyModeObject(PMType t)
        {
            this.g = Instantiate(neutrals[0]) as GameObject;
            this.g.transform.parent = parent;
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
    #endregion
}


