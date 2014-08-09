using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using Common;

public class GraphicsMenu : Menu
{
	private List<string> resolutions = new List<string>();
	private ComboBox comboBox = new ComboBox();
	private int ComboIndex = 0;

	private string CurResString
	{
		get
		{
			return Screen.currentResolution.width.ToString() + " x " + Screen.currentResolution.height.ToString();
		}
	}

	public GraphicsMenu(Rect menuArea) : base(menuArea)
	{
		for(int i = 0; i < Screen.resolutions.Length; i++)
		{
			Resolution r = Screen.resolutions[i];
			string res = r.width.ToString() + " x " + r.height.ToString();
			if(CurResString == res)
			{
				ComboIndex = i;
			}
			resolutions.Add(res);
		}
	}

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
		
		// Full Screen Toggle
		Screen.fullScreen = GUILayout.Toggle(Screen.fullScreen,"Full Screen");
		GUILayout.Label("Resolution: " + CurResString);

		// Screen Resolution Dropdown box
		GUILayout.Label("");
		int temp = comboBox.DrawComboBox(GUILayoutUtility.GetLastRect(), ComboIndex, resolutions.ToArray());
		if(temp != -1)
		{
			ComboIndex = temp;
			string[] newRes = resolutions[temp].Split("x".ToCharArray());
			Screen.SetResolution(int.Parse(newRes[0]), int.Parse(newRes[1]), Screen.fullScreen);
		}
		GUILayout.Space(100);
		
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
