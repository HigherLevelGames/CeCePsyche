using UnityEngine;
using System.Collections;

public class RunAroundOnScreen : MonoBehaviour {

    public Camera cam;
    MovementController controller;
    Animator anim;
    Vector2 onScreenPoint;
    void Start () {
        controller = GetComponent<MovementController>();
        anim = GetComponent<Animator>();
        PickOnScreenPoint();

    }
    void Update()
    {
        Vector2 p = transform.position;
        if (Mathf.Abs(onScreenPoint.x - p.x) > 0.5f)
        {
            controller.Right = onScreenPoint.x > p.x;
            controller.Left = onScreenPoint.x < p.x;
        }
    }
    public void PickOnScreenPoint()
    {
        onScreenPoint = cam.ViewportToWorldPoint(new Vector3(Random.value, 0,0));
        anim.SetTrigger("SetRunning");
    }

}
