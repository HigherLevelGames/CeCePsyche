using UnityEngine;
using System.Collections;

public class ChaseTarget : MonoBehaviour
{

    public Transform targetTransform;
    public bool Chase = false;
    MovementController controller;

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
        }
    }

    public void ChaseOff()
    {
        controller.Right = controller.Left = false;
        Chase = false;
    }
    public void ChaseOn()
    {
        Chase = true;
    }
}
