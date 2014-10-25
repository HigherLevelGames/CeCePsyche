using UnityEngine;
using System.Collections;

public class PsyModeController : MonoBehaviour
{
   
    public GameObject Player;
    public GameObject Other;
    public Texture[] textures;
    SpriteRenderer spr;
    Camera cam;
    float localScalar = 1.0f;

    float antiScalar { get { return 1.0f - localScalar; } }

    Color c = new Color(0, 0, 1.0f, 0.7f);
    bool psyOn;
    // Use this for initialization
    void Start()
    {
        cam = this.GetComponentInParent<Camera>();
        spr = this.GetComponent<SpriteRenderer>();
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
    }

    void OnGUI()
    {
        if (localScalar < 1.0f)
        {
            Vector2 p = cam.WorldToScreenPoint(Player.transform.position).ToVector2() + new Vector2(100, 100);
            p = new Vector2(Mathf.Clamp(p.x, 100, Screen.width - 100), Mathf.Clamp(Screen.height - p.y, 100, Screen.width - 100));
            Vector2 o = cam.WorldToScreenPoint(Other.transform.position).ToVector2();
            float xa = 75 * antiScalar;
            // Draw selectable stimuli next to Ceci
            GUI.Window(0, new Rect(p.x - xa, p.y - xa, xa * 2, xa * 2), PlayerWindow, "Stimuli", GUIStyle.none);
            // Draw unconditioned response next to target
            float xd = antiScalar * 25;
            GUI.Button(new Rect(o.x - xd, Screen.height - o.y - xd, xd * 2, xd * 2), "Response");
            // Draw environmental/unconditioned stimuli
            GUI.Window(1, new Rect(-5, -3, Screen.width + 10, Screen.height * 0.1f * antiScalar), EnvironmentWindow, "Environmental", GUIStyle.none);

        }         
    }

    void EnvironmentWindow(int id)
    {
        int numStim = 25;

        for (int i = 0; i < numStim; i++)
        {
            float r = ((float)i / numStim);
            float t = Mathf.Sin(r * Mathf.PI * 2);
            float x = 20 + r * Screen.width;
            float n = (i + 1) * 60 * t;
            float y = n * antiScalar - n + 10;
            GUI.Button(new Rect(x, y, 70, 55), i.ToString());
        }
    }

    void PlayerWindow(int id)
    {
        int numButtons = 3;
        int numDiff = 4;
        float rad = 34.0f * antiScalar;
        Vector2 center = new Vector2(75 * antiScalar, 80 * antiScalar);
        float wa = 60 * antiScalar;
        float wb = 30 * antiScalar;
        float bonusRotation = Mathf.PI * 2 * antiScalar - Mathf.PI * 0.75f;
        for(int i = 0; i < numButtons; i++)
        {
            float r = ((float)i / numButtons) * Mathf.PI * 2 * antiScalar + bonusRotation;
            float x = center.x + rad*(Mathf.Cos(r) - Mathf.Sin(r));
            float y = center.y + rad*(Mathf.Sin(r) + Mathf.Cos(r));
            Rect rec = new Rect(x - wb, y - wb, wa, wa);
            GUI.DrawTexture(rec, textures[i % numDiff]);
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
