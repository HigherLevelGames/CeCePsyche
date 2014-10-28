using UnityEngine;
using System.Collections;

public class PsyModeController : MonoBehaviour
{
   
    public GameObject Player;
    public GameObject Other;
    public Material HighlightSprite;
    public Texture[] Neutrals;
    public Texture[] Environmentals;
    public Texture[] Responses;
    public int SelectedStimuli = 0;
    PsyModeStimuli[] stimuli = new PsyModeStimuli[32];
    SpriteRenderer spr;
    Camera cam;
    float localScalar = 1.0f;

    float antiScalar { get { return 1.0f - localScalar; } }

    Color c = new Color(0, 0, 1.0f, 0.7f);
    bool psyOn;
    // Use this for initialization
    void Start()
    {
        for (int i = 0; i < stimuli.Length; i++)
            stimuli [i] = new PsyModeStimuli(new GameObject(), CCType.NeutralStimuli);
        cam = this.GetComponentInParent<Camera>();
        spr = this.GetComponentInChildren<SpriteRenderer>();
        spr.color = new Color(c.r, c.g, c.b, c.a * antiScalar);
    }

    void Update()
    {
        if (psyOn)
        {
            if (Input.GetKeyDown(KeyCode.Z))
                DisablePsyMode();
            localScalar = Mathf.Max(0, localScalar * 0.90f);
        } else
        {
            if (Input.GetKeyDown(KeyCode.Z))
                EnablePsyMode();
            localScalar = Mathf.Min(1.0f, 0.1f + localScalar * 1.02f);
        }
        Time.timeScale = localScalar;
        spr.color = new Color(c.r, c.g, c.b, c.a * antiScalar);
        for (int i = 0; i < stimuli.Length; i++)
        {
            switch (stimuli [i].Type)
            {
                default:
                    break;
            }
        }
    }

    void OnGUI()
    {

        if (localScalar < 1.0f)
        {
            float xa = 75 * antiScalar;
            Vector2 p = cam.WorldToScreenPoint(Player.transform.position).ToVector2();
            p += new Vector2(xa, xa + 20);
            p = new Vector2(Mathf.Clamp(p.x, 100, Screen.width - 100), Mathf.Clamp(Screen.height - p.y, 100, Screen.width - 100));

            // Draw selectable stimuli next to Ceci
            DrawNeutralStimuli(p);
            //GUI.Window(0, new Rect(p.x - xa, p.y - xa, xa * 2, xa * 2), PlayerWindow, "Stimuli", GUIStyle.none);
            // Draw unconditioned response next to target
            Vector2 o = cam.WorldToScreenPoint(Other.transform.position).ToVector2();
            float xd = antiScalar * 25;
            GUI.DrawTexture(new Rect(o.x - xd, Screen.height - o.y - xd, xd * 2, xd * 2), Responses [0]);
            // Draw environmental/unconditioned stimuli
            EnvironmentWindow();

        }         
    }

    void DrawNeutralStimuli(Vector2 p)
    {
        float rad = 34.0f * antiScalar;
        float wa = 60 * antiScalar;
        float wb = 30 * antiScalar;
        float bonusRotation = Mathf.PI * 2 * antiScalar - Mathf.PI * 0.75f;
        for (int i = 0; i < Neutrals.Length; i++)
        {
            float r = ((float)i / Neutrals.Length) * Mathf.PI * 2 * antiScalar + bonusRotation;
            float x = p.x + rad * (Mathf.Cos(r) - Mathf.Sin(r));
            float y = p.y + rad * (Mathf.Sin(r) + Mathf.Cos(r));
            Rect rec = new Rect(x - wb, y - wb, wa, wa);
            if (i == SelectedStimuli)
            {
                //glowSprite.Glow(rec, Neutrals [i], HighlightSprite);
            } else
                Graphics.DrawTexture(rec, Neutrals [i]);
        }
    }

    void EnvironmentWindow()
    {
        int numStim = 5;
        float w = 60.0f;
        float wa = w * antiScalar;

        for (int i = 0; i < numStim; i++)
        {
            float r = (float)(i + 1) / numStim;

            float wn = wa * r - w * r + w;
            float wb = wn * 0.5f;

            float x = 30 + i * w * 0.8f;
            float y = 30 + wb * (i % 2);

            GUI.DrawTexture(new Rect(x - wb, y - wb, wn, wn), Environmentals [i % Environmentals.Length]);
        }
    }

    void PlayerWindow(int id)
    {
        int numStim = 6;
        float rad = 34.0f * antiScalar;
        Vector2 center = new Vector2(75 * antiScalar, 80 * antiScalar);
        float wa = 60 * antiScalar;
        float wb = 30 * antiScalar;
        float bonusRotation = Mathf.PI * 2 * antiScalar - Mathf.PI * 0.75f;
        for (int i = 0; i < numStim; i++)
        {
            float r = ((float)i / numStim) * Mathf.PI * 2 * antiScalar + bonusRotation;
            float x = center.x + rad * (Mathf.Cos(r) - Mathf.Sin(r));
            float y = center.y + rad * (Mathf.Sin(r) + Mathf.Cos(r));
            Rect rec = new Rect(x - wb, y - wb, wa, wa);
            GUI.DrawTexture(rec, Neutrals [i % Neutrals.Length]);
        }

    }

    void EnablePsyMode()
    {
        psyOn = true;

    }

    void DisablePsyMode()
    {
        psyOn = false;
    }
}

public enum CCType
{
    NeutralStimuli,
    ConditionedStimuli,
    UnconditionedStimuli,
    UnconditionedResponse,
    ConditionedResponse
}

public class PsyModeStimuli
{
    public bool IsGlowing = false;
    public CCType Type;
    GameObject g;
    SpriteRenderer spr;
    bool reverse;
    float a;

    public PsyModeStimuli(GameObject g, CCType type)
    {
        this.g = g;
        this.spr = g.GetComponent<SpriteRenderer>();
        this.Type = type;
    }

    void DrawAt(Vector2 p)
    {

    }

    public void Glow()
    {
        if (a > 1)
            reverse = true;
        if (a < 0)
            reverse = false;
        if (reverse)
            a -= Time.unscaledDeltaTime;
        else
            a += Time.unscaledDeltaTime;
        spr.material.SetFloat("_GlowRate", a);
    }
}

public class GlowSprite
{
    float a;
    bool reverse;

    public GlowSprite()
    {

    }

}
