using UnityEngine;
using System.Collections;
using System;
using Common;

// Referenced: http://wiki.unity3d.com/index.php?title=PauseMenu
public class OptionMenu : Menu
{
	Rect box = new Rect(55,20,20,50);
	int selected = -1;
	string[] options = {
		"Audio",
		"Graphics",
		"Key Mapping",
		"Back"
	};
	
	public override void ShowMe()
	{
		selected = GUI.SelectionGrid(Utility.adjRect(box),selected,options,1);
		switch(selected)
		{
		case 0: // Audio
			OnChanged(EventArgs.Empty, 2);
			break;
		case 1: // Graphics
			OnChanged(EventArgs.Empty, 3);
			break;
		case 2: // Key Mapping
			OnChanged(EventArgs.Empty, 4);
			break;
		case 3: // Back
			OnChanged(EventArgs.Empty, 0);
			break;
		default:
			break;
		}
		selected = -1;
	}
	
	/*
	void KeyMapControl()
	{
		GUILayout.BeginHorizontal();
		int i = 0;
		foreach(KeyCode c in (KeyCode[])Enum.GetValues(typeof(KeyCode)))
		{
			if(i%25 == 0)
			{
				GUILayout.BeginVertical();
			}
			GUILayout.Button(c.ToString());
			if(i%25 == 24)
			{
				GUILayout.EndVertical();
			}
			i++;
		}
		GUILayout.EndVertical();
		GUILayout.EndHorizontal();
	}//*/
}
