using UnityEngine;
using System.Collections;

public class PauseScreen : MonoBehaviour
{
	bool showPause = false; // flag

	// Use this for initialization
	void Start () {}
	
	// Update is called once per frame
	void Update ()
	{
		if(Input.GetKeyDown(KeyCode.P)) // Input.GetButtonDown("Start")
		{
			showPause = !showPause;
			if(showPause)
			{
				Time.timeScale = 0.0f;
			}
			else
			{
				Time.timeScale = 1.0f;
			}
		}
	}

	void OnGUI()
	{
		if(showPause)
		{
			GUI.Box(new Rect(200,200,200,200),"Paused");
		}
	}
}
