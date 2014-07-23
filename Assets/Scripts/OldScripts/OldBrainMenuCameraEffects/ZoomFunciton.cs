using UnityEngine;
using System.Collections;

public class ZoomFunciton : MonoBehaviour {
	
	private SmoothLookAt lookAt;
	
	// Use this for initialization
	void Start () {
		PlayerPrefs.SetInt("camera", 1);
		lookAt = GetComponent<SmoothLookAt>();
	}
	
	// Update is called once per frame
	void Update () {
		if(PlayerPrefs.GetInt("camera") == 0)
		{
			GetComponent<SmoothLookAt>().enabled = true;
			camera.orthographicSize -= 5 * (Time.deltaTime/4);
		}
	}
}
