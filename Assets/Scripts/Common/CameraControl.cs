using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CameraControl : MonoBehaviour
{
    private MovementController control; // is the player facing left or right?
    private Transform playerTransform;
    private PolygonCollider2D Boundary; // the camera will remain in here, or else.
    public float xOffset = 0.7f; // how far in front of the player the camera will follow
    public float yOffset = 0.0f; // how far above or below the camera will naturally follow the player
    public float SpringSpeed = 2.0f; // how quick the camera will follow the player, 0 = will not follow

    Vector2 Target; // The camera will be attracted to this point

    public void Initialize(GameObject player, PolygonCollider2D bounds)
    {
        playerTransform = player.transform;
        control = player.GetComponent<MovementController>();
        Target = new Vector2();
        Boundary = bounds;
        Boundary.isTrigger = true;

    }

    void FixedUpdate()
    { 
        int face = control.isFacingRight ? 1 : -1;
        Vector2 cp = camera.transform.position;
        Vector2 p = playerTransform.position;
        Vector2 off = camera.ViewportToScreenPoint(new Vector3(xOffset * face, yOffset, 0));

        Target = InBoundsPoint(p + off * 0.008f);
        cp += (Target - cp) * Time.deltaTime * SpringSpeed; // move toward the target at a certain speed
        camera.transform.position = new Vector3(cp.x, cp.y, camera.transform.position.z);           
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