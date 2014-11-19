using UnityEngine;
using System.Collections;

public class SpitFood : MonoBehaviour
{
    public GameObject Food;
    public Camera cam;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mp = cam.ScreenToWorldPoint(Input.mousePosition);
            if (collider2D.bounds.Contains(mp))
            {
                Animator ani = GetComponentInChildren<Animator>();
                ani.SetTrigger("SpitFood");
                PlaySound ps = GetComponent<PlaySound>();
                ps.PlayAudioOnce();
            }
        }
    }

    public void Spit()
    {
        GameObject g = Instantiate(Food) as GameObject;
        g.transform.position = transform.position;
        g.transform.parent = transform;
        g.rigidbody2D.AddForce(new Vector2(-100, 30));
        g.rigidbody2D.AddTorque(10f);
    }
}
