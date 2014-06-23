using UnityEngine;
using System.Collections;

public class ParallaxScrolling : MonoBehaviour {
	
	// Use this for initialization
	public Transform[] LayersTransform;
	public Transform ScrollTransform;

	public Camera cam;
	bool scrollPaused = false;
	Vector3 Scroll; // for easy 2D scroll manipulation
	void Start () {
		Scroll = ScrollTransform.position;
	}

	// Update is called once per frame, which is funny, because Draw is called once per frame...
	void Update () {
		if (Input.GetKeyDown (KeyCode.Space))
						scrollPaused = !scrollPaused;
		if (!scrollPaused) {
						Vector3 mp = Input.mousePosition;
						float scrollX = Scroll.x; // separate the scroll elements so they can bey used separately for certain layers.
						float scrollY = Scroll.y;
						float scrollZ = Scroll.z;
						Vector3 screenCenter = new Vector3 (Screen.width * 0.5f, Screen.height * 0.5f, 0f);
						if (Vector3.Distance (mp, screenCenter) > 100) {
								scrollX -= (mp.x - screenCenter.x) * Time.deltaTime * 0.01f;
								scrollY -= (mp.y - screenCenter.y) * Time.deltaTime * 0.01f;
					
								Scroll = new Vector3 (scrollX, scrollY, scrollZ);
								//cam.transform.LookAt (Scroll);
								//cam.transform.position = new Vector3 (Scroll.x, Scroll.y, cam.transform.position.z);
								LayersTransform [0].position = Scroll * 0.1f; // the farground or true background, only scrolls every so slightly TODO: lock sides to screen
								LayersTransform [1].position = Scroll * 0.75f;
								LayersTransform [2].position = Scroll;
								LayersTransform [3].position = Scroll * 0.9f;
						}
				}

	}
}
