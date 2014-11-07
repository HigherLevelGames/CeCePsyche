using UnityEngine;
using System.Collections;

public static class ExtensionMethods
{
    public static Vector3 ToVector3(this Vector2 v)
    {
        return new Vector3(v.x, v.y, 0);
    }

    public static Vector2 ToVector2(this Vector3 v)
    {
        return new Vector2(v.x, v.y);
    }

    public static Vector2[] ToVector2(this Vector3[] a)
    {
        Vector2[] newArray = new Vector2[a.Length];
        for (int i = 0; i < a.Length; i++)
            newArray [i] = new Vector2(a [i].x, a [i].y);
        return newArray;
    }

    public static Vector3[] ToVector3(this Vector2[] a)
    {
        Vector3[] newArray = new Vector3[a.Length];
        for (int i = 0; i < a.Length; i++)
            newArray [i] = new Vector3(a [i].x, a [i].y, 0);
        return newArray;
    }
}