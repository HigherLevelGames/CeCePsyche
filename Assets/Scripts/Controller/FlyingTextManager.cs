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
        GameObject o = Instantiate(FlyingTextPrefab, p, Quaternion.identity) as GameObject;
        o.transform.SetParent(CanvasTransform, false);
        o.GetComponent<FlyingText>().WorldSpacePosition = location;
        o.GetComponentInChildren<Text>().text = text;
    }
}
