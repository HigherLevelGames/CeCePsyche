using UnityEngine;
using System.Collections;
using System;
using System.IO;
using Common;

// Unity Notes:
//Debug.Log("Data Path: " + Application.dataPath);
/* // Application.dataPath:
		Unity Editor: <path to project folder>/Assets
		Mac player: <path to player app bundle>/Contents
		iPhone player: <path to player app bundle>/<AppName.app>/Data
		Win player: <path to executablename_Data folder>
		Web player: The absolute url to the player data file folder (without the actual data file name)
		Flash: The absolute url to the player data file folder (without the actual data file name)
		Note that the string returned on a PC will use a forward slash as a folder separator.
		//*/
//Debug.Log("Persistent Data Path: " + Application.persistentDataPath);
// persistentDataPath: where data is expected to be kept between runs

public class LoadMenu : Menu
{
	private Profile mainProfile;
	private int prevSelected;
	
	public LoadMenu(Rect menuArea) : base(menuArea)
	{
		prevSelected = selected;
		
		/*// test stuff
		for(int i = 0; i < 6; i++)
		{
			mainProfile = new Profile();
			mainProfile.PlayerName = "TestSave" + i;
			string savePath = Path.Combine(Application.dataPath, "Saves");
			mainProfile.Save(savePath);
		}//*/
		
		// get all save files
		string myPath = Application.persistentDataPath;//Path.Combine(Application.dataPath, "Saves"); // TODO: switch to Application.persistentDataPath for final build
		//System.IO.Directory.CreateDirectory(myPath);
		DirectoryInfo dir = new DirectoryInfo(myPath);
		FileInfo[] info = dir.GetFiles("*.xml");
		
		// place saves into string[] to display to user
		options = new string[info.Length];
		for(int i = 0; i < info.Length; i++)
		{
			options[i] = info[i].Name;
		}
	}
	
	public override void ShowMe()
	{
		base.ShowMe();
		
		// update save info
		if(prevSelected != selected)
		{
			string myPath = Path.Combine(Application.dataPath, "Saves");
			myPath = Path.Combine(myPath,options[selected]);
			mainProfile = Profile.Load(myPath);
		}
		
		GUILayout.BeginArea(Utility.adjRect(new Rect(35, 10, 55, 80)));
		
		// show save info
		if(mainProfile != null)
		{
			string displayText = mainProfile.PlayerName;
			displayText += "\nUnlocked Level: " + mainProfile.UnlockedLevel;
			displayText += (mainProfile.EduMode) ? "\nMode: Educational Mode" : "\nMode: Story Mode";
			displayText += "\nTotal Time Played: " + mainProfile.TotalTimePlayed;
			GUILayout.Box(displayText);
		}
		
		if(GUILayout.Button("Back"))
		{
			OnChanged(EventArgs.Empty, 0);
		}
		if(GUILayout.Button("Play"))
		{
			// load game & play
			GameObject.Find("ProfileContainer").SendMessage("SetProfile", mainProfile);
			Application.LoadLevel("Brain Menu");
		}
		
		GUILayout.EndArea();
		prevSelected = selected;
	}
}
