using UnityEngine;
using System;
using System.Collections;
using Common;

public class GraphicsMenu : Menu
{
	public GraphicsMenu(Rect menuArea) : base(menuArea) { }

	public override void ShowMe()
	{
		GUILayout.BeginArea(Utility.adjRect(box));

		// Quality Level
		GUILayout.BeginHorizontal();
		GUILayout.Label("Quality: " + QualitySettings.names[QualitySettings.GetQualityLevel()]);
		if (GUILayout.Button("Decrease"))
		{
			QualitySettings.DecreaseLevel();
		}
		if (GUILayout.Button("Increase"))
		{
			QualitySettings.IncreaseLevel();
		}
		GUILayout.EndHorizontal();
		
		// Full Screen & Screen Resolution
		Screen.fullScreen = GUILayout.Toggle(Screen.fullScreen,"Full Screen");
		GUILayout.Label("Resolution: " + Screen.currentResolution.width + " x " + Screen.currentResolution.height);
		foreach(Resolution r in Screen.resolutions)
		{
			string res = r.width.ToString() + " x " + r.height.ToString();
			if(GUILayout.Button (res))
			{
				Screen.SetResolution(r.width, r.height, Screen.fullScreen);
			}
		}

		// Buttons
		if(GUILayout.Button("Apply"))
		{
			// apply and save settings to PlayerPrefs
			OnChanged(EventArgs.Empty, 1);
		}
		
		if(GUILayout.Button("Back"))
		{
			OnChanged(EventArgs.Empty, 1);
		}

		GUILayout.EndArea();
	}
}
