using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FlyingTextManager : MonoBehaviour
{
    public static FlyingTextManager data;
    public RectTransform CanvasTransform;
    public GameObject FlyingTextPrefab;
    
    void Awake()
    {
        if (data == null)
            data = this;
    }

    public void SpawnTextAt(Vector2 location, string text)
    {
        Vector2 p = Camera.main.WorldToScreenPoint(location);
        Debug.Log(p);
        GameObject o = Instantiate(FlyingTextPrefab, p, Quaternion.identity) as GameObject;
        o.transform.parent = CanvasTransform;
        o.GetComponentInChildren<Text>().text = text;
    }
}
