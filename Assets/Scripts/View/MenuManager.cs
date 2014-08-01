using UnityEngine;
using System; // for EventArgs
using System.Collections;
using Common;

[RequireComponent (typeof(HUD))]
public class MenuManager : MonoBehaviour
{
	private Menu[] menus = new Menu[6];
	private int curMenu = 0;

	// private float savedTimeScale;
	bool showPause = false; // flag

	// basic boxes
	Rect winRect = new Rect(10,10,80,80);
	Rect levelNameBox = new Rect(10,5,50,10);
	Rect menuBox = new Rect(55, 20, 20, 50);

	// edu mode boxes
	Rect urBox = new Rect(5,20,45,25);
	Rect usBox = new Rect(5,45,45,25);
	Rect usIconBox = new Rect(35,50,10,15);

	// Use this for initialization
	void Start ()
	{
		menus[0] = new PauseMenu(menuBox);
		menus[1] = new OptionMenu(menuBox);
		menus[2] = new AudioMenu(menuBox);
		menus[3] = new GraphicsMenu(menuBox);
		menus[4] = new RebindingMenu(menuBox);
		//menus[5] = new ExtrasMenu(menuBox);

		for(int i = 0; i < 5; i++)
		{
			menus[i].Changed += new ChangedEventHandler(SwapMenu);
		}
	}

	// Update is called once per frame
	void Update ()
	{
		if(RebindableInput.GetKeyDown("Pause"))
		{
			TogglePause();
		}
		if(showPause)
		{
			menus[curMenu].Update();
		}
	}

	void OnGUI()
	{
		if(showPause)
		{
			winRect = Utility.unadjRect(GUI.Window(0, Utility.adjRect(winRect), ShowWindow, "Paused"));
		}
	}

	void ShowWindow(int id)
	{
		GUI.Box(Utility.adjRect(levelNameBox), "Level Name: " + Application.loadedLevelName);
		
		// Pause Menu
		menus[curMenu].ShowMe();
		
		if(Utility.GetBool("EduMode"))
		{
			GUI.Box(Utility.adjRect(urBox), "Unconditioned Response: " + "0" + " out of 3");
			GUI.Box(Utility.adjRect(usBox), "Unconditioned Stimulus:");
			GUI.Box(Utility.adjRect(usIconBox), "Icon");
		}

		GUI.DragWindow();
	}

	// Event Handler Method (this is called whenever the event is fired)
	private void SwapMenu(object sender, EventArgs e, int index) 
	{
		// JNN: using a switch just to make sure everything's working correctly
		// in case more menus are implemented/added later on
		switch(index)
		{
		case -1: // resume, restart, or quit
			TogglePause();
			break;
		case 0: // pause menu
			curMenu = index;
			break;
		case 1: // options
			curMenu = index;
			break;
		case 2: // audio
			curMenu = index;
			break;
		case 3: // graphics
			curMenu = index;
			break;
		case 4: // keymapping
			curMenu = index;
			break;
		default:
			Debug.Log ("An Event was fired to swap the current menu");
			break;
		}
	}

	void TogglePause()
	{
		showPause = !showPause;
		if(showPause)
		{
			// pause the game
			//savedTimeScale = Time.timeScale;
			Time.timeScale = 0.0f;
			
			this.GetComponent<HUD>().enabled = false;
			
			//AudioListener.pause = true;
			// Background Music should have this:
			//this.audio.ignoreListenerVolume = true;
		}
		else
		{
			// unpause the game
			Time.timeScale = 1.0f; // = savedTimeScale;
			this.GetComponent<HUD>().enabled = true;
			
			//AudioListener.pause = false;
		}
	}
	
	/*
	bool IsGamePaused()
	{
		return (Time.timeScale == 0);
	}
	
	void OnApplicationPause(bool pause)
	{
		if (IsGamePaused())
		{
			AudioListener.pause = true;
		}
	}
	//*/
}