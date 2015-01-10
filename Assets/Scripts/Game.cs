using UnityEngine;
using System.Collections;

public class Game : MonoBehaviour
{
    public static Game singleton;

    public static PolygonCollider2D CameraBounds;
   
    public GameObject CeciPrefab;
    public GameObject GameCameraPrefab;

    public static GameObject Ceci;
    public static GameObject GameCamera;
    void Awake()
    {
        if (singleton == null)
            singleton = this;
    }

    public void Initialize()
    {
        Ceci = Instantiate(CeciPrefab) as GameObject;
        GameCamera = Instantiate(GameCameraPrefab) as GameObject;
        GameCamera.GetComponent<CameraControl>().Initialize(Ceci, CameraBounds);
        Debug.Log("Starting Scene");
    }
}
