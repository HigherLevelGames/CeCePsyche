using UnityEngine;
using System.Collections;
using System;

// Referenced: http://wiki.unity3d.com/index.php?title=PauseMenu
public class OptionMenu : MonoBehaviour
{
	private float savedTimeScale;
	public bool showOptions = true;
	private int toolbarInt = 0;
	private string[]  toolbarstrings =  {"Audio","Graphics","Key Mapping"};
	
	// window vars
	int winW = 500;
	int winH = 350;
	Rect winRect = new Rect(0,0,0,0);

	void Start()
	{
		Time.timeScale = 1;
		//winRect = new Rect((Screen.width - winW) / 2, (Screen.height - winH) / 2, winW, winH);
		winRect = new Rect(0,0,Screen.width,Screen.height);
		//PauseGame();
	}

	void LateUpdate ()
	{
		if (Input.GetKeyDown("escape")) 
		{
			showOptions = !showOptions;
			if(showOptions)
			{
				PauseGame();
			}
			else
			{
				UnPauseGame();
			}
		}
	}
	
	void OnGUI ()
	{
		if (IsGamePaused() && showOptions)
		{
			winRect = GUI.Window (0,winRect, ShowToolbar,"Options");
		}
	}

	void ShowToolbar(int id)
	{
		toolbarInt = GUILayout.Toolbar (toolbarInt, toolbarstrings);
		switch (toolbarInt) {
		case 0: VolumeControl(); break;
		case 1: Qualities(); break;
		case 2: KeyMapControl(); break;
		}
		GUI.DragWindow();
	}

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
	}

	void Qualities()
	{
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

		// Full Screen & Resolution
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
	}

	void VolumeControl()
	{
		GUILayout.Label("Volume");
		AudioListener.volume = GUILayout.HorizontalSlider(AudioListener.volume, 0, 1);
	}
	
	void PauseGame()
	{
		savedTimeScale = Time.timeScale;
		Time.timeScale = 0;
		AudioListener.pause = true;
		// Background Music should have this:
		//this.audio.ignoreListenerVolume = true;
		showOptions = true;
	}
	
	void UnPauseGame()
	{
		Time.timeScale = savedTimeScale;
		AudioListener.pause = false;
		showOptions = false;
	}
	
	bool IsGamePaused()
	{
		return (Time.timeScale == 0);
	}
	
	void OnApplicationPause(bool pause)
	{
		if (IsGamePaused()) {
			AudioListener.pause = true;
		}
	}
}
