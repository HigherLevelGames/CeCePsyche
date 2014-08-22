using UnityEngine;
using System.Collections;
using System;
using System.IO;
using Common;

public class GameModeMenu : Menu
{
	private string gameName = "Cecilia";
	
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
		GUILayout.BeginArea(Utility.adjRect(box));
		
		// name to use when saving
		gameName = GUILayout.TextField(gameName);
		
		// select game mode to play
		GUILayout.BeginHorizontal();
		if(GUILayout.Button(options[0])) // Story Mode
		{
			CreateNewGame(false);
		}
		if(GUILayout.Button(options[1])) // Edu mode
		{
			CreateNewGame(true);
		}
		GUILayout.EndHorizontal();
		
		// back button
		if(GUILayout.Button(options[2]))
		{
			OnChanged(EventArgs.Empty, 0);
		}
		
		GUILayout.EndArea();
	}
	
	void CreateNewGame(bool isEdu)
	{
		// create a new profile
		Profile mainProfile = new Profile();
		mainProfile.PlayerName = gameName;
		mainProfile.EduMode = isEdu;
		
		//Utility.SetBool("EduMode", isEdu);
		
		// save the profile
		string savePath = Path.Combine(Application.dataPath, "Saves"); // TODO: switch to Application.persistentDataPath for final build
		mainProfile.Save(savePath);
		
		// start the game
		GameObject.Find("ProfileContainer").SendMessage("SetProfile", mainProfile);
		Application.LoadLevel("Brain Menu");
	}
}
