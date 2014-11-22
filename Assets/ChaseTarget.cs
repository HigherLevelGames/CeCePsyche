using UnityEngine;
using System.Collections;

public class ChaseTarget : MonoBehaviour
{

    public Transform targetTransform;
    MovementController controller;
    public bool Chase = true;

    void Start()
    {
        controller = GetComponent<MovementController>();
    }
   
    void Update()
    {
        if (Chase)
        {
            Vector2 t = targetTransform.position;
            Vector2 p = transform.position;
            if (Mathf.Abs(t.x - p.x) > 1f)
            {
                controller.Right = t.x > p.x;
                controller.Left = t.x < p.x;
            }
        } else
            controller.Right = controller.Left = false;
    }

    public void ChaseOff()
    {
        Chase = false;
    }
}
