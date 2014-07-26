using UnityEngine;
using System;
using System.Collections;
using Common;

public class PauseMenu : Menu
{
	// selection grid vars
	Rect box = new Rect(55,20,20,50);
	int selected = -1;
	string[] options = {
		"Resume",
		"Restart",
		"Options",
		"Extras",
		"Exit"
	};
	
	public override void ShowMe()
	{
		selected = GUI.SelectionGrid(Utility.adjRect(box),selected,options,1);
		
		switch(selected)
		{
		case 0: // Resume
			OnChanged(EventArgs.Empty, -1); // toggle pause
			break;
		case 1: // Restart
			OnChanged(EventArgs.Empty, -1); // toggle pause
			Application.LoadLevel(Application.loadedLevel);
			//Application.LoadLevel(Application.loadedLevelName);
			break;
		case 2: // Options
			OnChanged(EventArgs.Empty, 1);
			break;
		case 3: // Extras
			//for now sets edu mode on/off
			Utility.SetBool("EduMode", !Utility.GetBool("EduMode"));
			
			Debug.Log ("TODO: add Extra's Menu");
			OnChanged(EventArgs.Empty, -2);
			break;
		case 4:
			OnChanged(EventArgs.Empty, -1); // toggle pause
			Application.LoadLevel("Brain Menu");
			break;
		default:
			break;
		}
		selected = -1;
	}
}
