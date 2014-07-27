using UnityEngine;
using System;
using System.Collections;
using Common;

public class StartMenu : Menu
{
	public StartMenu(Rect menuArea) : base(menuArea)
	{
		options = new string[] {
			"Story Mode",
			"Educational Mode",
			"Concept",
			"Options",
			"Credits",
			"Quit"
		};
	}

	protected override void PressedEnter()
	{
		switch(selected)
		{
		case 0: // Story Mode
			Utility.SetBool("EduMode", false);
			Application.LoadLevel("Brain Menu");
			break;
		case 1: // Tutorial
			Utility.SetBool("EduMode", true);
			Application.LoadLevel("Brain Menu");
			break;
		case 2: // Concept
			Application.LoadLevel("Concept");
			break;
		case 3: // Options
			OnChanged(EventArgs.Empty, 1);
			break;
		case 4: // Credits
			Application.LoadLevel("Credits"); 
			break;
		case 5: // Quit
			Debug.Log ("Quitting Game");
			Application.Quit();
			break;
		default:
			break;
		}
		selected = -1;
	}
}
