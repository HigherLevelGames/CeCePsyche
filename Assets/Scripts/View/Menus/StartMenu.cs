using UnityEngine;
using System;
using System.Collections;
using Common;

public class StartMenu : Menu
{
	public StartMenu(Rect menuArea) : base(menuArea)
	{
		options = new string[] {
			"New Game",
			"Load Game",
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
		case 0: // New Game
			OnChanged(EventArgs.Empty, 5);
			break;
		case 1: // Load Game
			OnChanged(EventArgs.Empty, 6);
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
