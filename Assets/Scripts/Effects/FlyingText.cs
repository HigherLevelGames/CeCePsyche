using UnityEngine;
using System.Collections;

public class FlyingText : MonoBehaviour
{

    [HideInInspector]
    public Vector2
        WorldSpacePosition;

    void Update()
    {
        transform.position = Camera.main.WorldToScreenPoint(WorldSpacePosition);
    }
}
