using UnityEngine;
using System.Collections;
using Common;

[RequireComponent (typeof(HUD))]
public class PauseMenu : MonoBehaviour
{
	bool showPause = false; // flag
	Rect box = new Rect(10,10,80,80);

	// basic boxes
	Rect levelNameBox = new Rect(30,15,40,10);
	Rect resumeBox = new Rect(65,30,20,10);
	Rect restartBox = new Rect(65,40,20,10);
	Rect extraBox = new Rect(65,50,20,10);
	Rect optionBox = new Rect(65,60,20,10);
	Rect exitBox = new Rect(65,70,20,10);

	// edu mode boxes
	Rect urBox = new Rect(15,30,45,25);
	Rect usBox = new Rect(15,55,45,25);
	Rect usIconBox = new Rect(45,60,10,15);

	// Use this for initialization
	void Start () {}
	
	// Update is called once per frame
	void Update ()
	{
		if(Input.GetKeyDown(KeyCode.P)) // Input.GetButtonDown("Start")
		{
			TogglePause();
		}
	}

	void TogglePause()
	{
		showPause = !showPause;
		if(showPause)
		{
			Time.timeScale = 0.0f;
			this.GetComponent<HUD>().enabled = false;
		}
		else
		{
			Time.timeScale = 1.0f;
			this.GetComponent<HUD>().enabled = true;
		}
	}

	void OnGUI()
	{
		if(showPause)
		{
			GUI.Box(Utility.adjRect(box), "Paused");

			GUI.Box(Utility.adjRect(levelNameBox), "Level Name: " + Application.loadedLevelName);
        	if(GUI.Button(Utility.adjRect(resumeBox), "Resume"))
			{
				TogglePause();
			}
			if(GUI.Button(Utility.adjRect(restartBox), "Restart"))
			{
				Application.LoadLevel(Application.loadedLevel);
				//Application.LoadLevel(Application.loadedLevelName);
				TogglePause();
			}
			if(GUI.Button(Utility.adjRect(extraBox), "Extras"))
			{
				Debug.Log ("TODO: add Extra's Menu");
			}
			if(GUI.Button(Utility.adjRect(optionBox), "Options"))
			{
				//for now sets edu mode on/off
				Utility.SetBool("EduMode", !Utility.GetBool("EduMode"));
				Debug.Log ("TODO: add Option's Menu");
				// SFX/Music Volume
				// Screen Resolution
				// Key/Button Mapping
			}
			if(GUI.Button(Utility.adjRect(exitBox), "Exit"))
			{
				Application.LoadLevel("BrainMenu");
			}

			if(Utility.GetBool("EduMode"))
			{
				GUI.Box(Utility.adjRect(urBox), "Unconditioned Response: " + "0" + " out of 3");
				GUI.Box(Utility.adjRect(usBox), "Unconditioned Stimulus:");
				GUI.Box(Utility.adjRect(usIconBox), "Icon");
			}
		}
	}
}
