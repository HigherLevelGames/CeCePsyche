using UnityEngine;
using System.Collections;

public class PsyModeController : MonoBehaviour
{
   
    public GameObject Player;
    public GameObject Other;
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
            localScalar = Mathf.Max(0, localScalar * 0.9f);
        } else
        {
            if (Input.GetKeyDown(KeyCode.Z))
                EnablePsyMode();
            localScalar = Mathf.Min(1.0f, 0.1f + localScalar * 1.1f);
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
            GUI.Window(0, new Rect(p.x - xa, p.y - xa, xa * 2, xa * 2), PlayerWindow, "Stimuli");
            // Draw unconditioned response next to target
            float xd = antiScalar * 25;
            GUI.Button(new Rect(o.x - xd, Screen.height - o.y - xd, xd * 2, xd * 2), "Response");
            // Draw!
            GUI.Window(1, new Rect(-5, -3, Screen.width + 10, Screen.height * 0.1f * antiScalar), EnvironmentWindow, "Environmental");

        }         
    }

    void EnvironmentWindow(int id)
    {
        GUILayout.Button("LIGHTNING BOLTS FROM HIS ARSE", new GUILayoutOption[]
        {
            GUILayout.MaxWidth(100),
            GUILayout.MaxHeight(100)
        });
    }

    void PlayerWindow(int id)
    {
        GUILayout.Button("Hello");
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
