using UnityEngine;
using System.Collections;

public class Subconcious : MonoBehaviour
{
	// Use this for initialization
	void Start () { }
	
	// Update is called once per frame
	void Update ()
	{
		Vector3 screenPos = new Vector3(Input.mousePosition.x, Input.mousePosition.y);
		screenPos.z = 10.0f; // distance from camera
		Vector3 pos = Camera.main.ScreenToWorldPoint(screenPos);
		this.transform.position = pos;
	}
}
