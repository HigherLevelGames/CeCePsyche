using UnityEngine;
using System.Collections;
using Util;

namespace Util
{
	public class Utility
	{
		public static Rect adjRect(Rect r)
		{
			return new Rect(r.x * Screen.width / 100,
					r.y * Screen.height / 100,
			        r.width * Screen.width / 100,
			        r.height * Screen.height / 100);
		}
	}
}

public class TitleMenu : MonoBehaviour
{
	public string[] buttonNames = {"Story Mode", "Tutorial", "Concept", "Options", "Credits", "Quit"};
	public Rect grid = new Rect(0,0,0,0);
	private int curSelection = -1;
	private bool isEntered = false;

	// Use this for initialization
	void Start () { }
	
	// Update is called once per frame
	void Update ()
	{
		// Stretch backgrownd to Screen Size
		if(this.guiTexture != null)
		{
			int w = Screen.width;
			int h = Screen.height;
			this.guiTexture.pixelInset = new Rect(-w/2, -h/2, w, h);
		}

		// up and down keys to control menu
		if(Input.GetKeyDown(KeyCode.DownArrow))
		{
			curSelection = (curSelection + 1) % buttonNames.Length;
		}
		if(Input.GetKeyDown(KeyCode.UpArrow))
		{
			if(curSelection > 0)
			{
				curSelection--;
			}
			else
			{
				curSelection = buttonNames.Length-1;
			}
		}

		// space/enter to select
		if(Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return))
		{
			isEntered = true;
		}

		// perform selected action (hard coded for now, since levels are named differently than the button names)
		// also, not sure if we want the settings menu to be in a separate scene or the same one
		if(isEntered)
		{
			isEntered = false;
			switch(curSelection)
			{
			case 0: // Story Mode
				Application.LoadLevel("Introduction");
				break;
			case 1: // Tutorial
				Application.LoadLevel("BrainMenu");
				break;
			case 2: // Concept
				Application.LoadLevel("Credits"); // no clue
				break;
			case 3: // Options
				// SFX
				// BG music
				// key mapping control scheme: up/down/left/right, jump, interact, ability
				// ability can be set at brain menu
				break;
			case 4: // Credits
				Application.LoadLevel("Concept"); // no clue
				break;
			case 5: // Quit
				Application.Quit();
				break;
			default:
				break;
			}
		}
	}

	// buttons
	void OnGUI()
	{
		// draw selection grid buttons
		curSelection = GUI.SelectionGrid(Utility.adjRect(grid), curSelection, buttonNames, 1);
		// left click event same as enter event
		if(Input.GetMouseButtonUp(0) && Utility.adjRect(grid).Contains(Input.mousePosition))
		{
			// http://docs.unity3d.com/Manual/ExecutionOrder.html
			// the check is done here due to the ordering of Unity Events
			// i.e. OnGUI() draws multiple times before Update() is,
			// so the click detection might be lost if we don't check for it immediately
			isEntered = true;
		}
	}
}
