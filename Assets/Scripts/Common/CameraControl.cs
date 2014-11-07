using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CameraControl : MonoBehaviour
{
    public Camera MyCamera; // the camera to control
    public Transform PlayerTransform; // the players position can be found here
    public PolygonCollider2D Boundary; // the camera will remain in here, or else.
    public float xOffset = 0.7f; // how far in front of the player the camera will follow
    public float yOffset = 0.0f; // how far above or below the camera will naturally follow the player
    public float SpringSpeed = 2.0f; // how quick the camera will follow the player, 0 = will not follow
    public MovementController LeftRightController; // is the player facing left or right?

    Vector2 Target; // The camera will be attracted to this point

    void Start()
    {
        Target = new Vector2();
        Boundary.isTrigger = true;
    }

    void FixedUpdate()
    { 
        int face = LeftRightController.isFacingRight ? 1 : -1;
        Vector2 cp = MyCamera.transform.position;
        Vector2 p = PlayerTransform.position;
        Vector2 off = MyCamera.ViewportToScreenPoint(new Vector3(xOffset * face, yOffset, 0));

        Target = InBoundsPoint(p + off * 0.008f);
        cp += (Target - cp) * Time.deltaTime * SpringSpeed; // move toward the target at a certain speed
        MyCamera.transform.position = new Vector3(cp.x, cp.y, MyCamera.transform.position.z);           
    }

    Vector2 InBoundsPoint(Vector2 p)
    {
        Vector2 v = new Vector2();
        if (Boundary.OverlapPoint(p))
            v = p;
        else
        {
            float dist = 20;
            Vector2 pos = Boundary.gameObject.transform.position.ToVector2();
            Vector2[] path = Boundary.GetPath(0);
            for (int i = 0; i < path.Length; i++)
            {
                Vector2 vec = new Vector2(); 
                if (i == path.Length - 1)
                    vec = ClosestPointOnLine(path [0], path [path.Length - 1], p - pos);
                else
                    vec = ClosestPointOnLine(path [i], path [i + 1], p - pos);
                float newDist = Vector2.Distance(vec, p);
                if (newDist < dist)
                {
                    v = vec;
                    dist = newDist;
                }   
            }
        }
        return v;
    }

    Vector2 ClosestPointOnLine(Vector2 a, Vector2 b, Vector2 p)
    {
        Vector2 v1 = (b - a);
        Vector2 v2 = (p - a);
        float dot = Mathf.Clamp(Vector2.Dot(v2.normalized, v1.normalized) * v2.magnitude, 0, v1.magnitude);
        
        return a + v1.normalized * dot;
    }
}