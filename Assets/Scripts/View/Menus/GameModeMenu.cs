using UnityEngine;
using System.Collections;
using System;
using Common;

public class GameModeMenu : Menu
{
	private string gameName = "";

	public GameModeMenu(Rect menuArea) : base(menuArea)
	{
		options = new string[] {
			"Story Mode",
			"Educational Mode",
			"Back"
		};
	}

	public override void ShowMe()
	{
		// name to use when saving
		gameName = GUILayout.TextField(gameName);

		// select game mode to play
		GUILayout.BeginHorizontal();
		if(GUILayout.Button(options[0])) // Story Mode
		{
			Utility.SetBool("EduMode", false);
			Application.LoadLevel("Brain Menu");
		}
		if(GUILayout.Button(options[1])) // Edu mode
		{
			Utility.SetBool("EduMode", true);
			Application.LoadLevel("Brain Menu");
		}
		GUILayout.EndHorizontal();

		// back button
		if(GUILayout.Button(options[2]))
		{
			OnChanged(EventArgs.Empty, 0);
		}
	}

	/*
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
		case 2: // Back
			OnChanged(EventArgs.Empty, 0);
			break;
		default:
			break;
		}
		selected = -1;
	}//*/
}
