using UnityEngine;
using System;
using System.Collections;
using Common;

// Other Audio Settings to Consider:
//AudioSettings.outputSampleRate;
//AudioSettings.GetDSPBufferSize(out int bufferLength, out int numBuffers);
//AudioSettings.SetDSPBufferSize(int bufferLength, int numBuffers);

//AudioSettings.speakerMode = AudioSpeakerMode.Stereo; // default
//Raw, Mono, Stereo, Quad, Surround, Mode5point1, Mode7point1, Prologic
//see http://docs.unity3d.com/ScriptReference/AudioSpeakerMode.html

public class AudioMenu : Menu
{
	Rect box = new Rect(55,20,20,50);
	float masterVolume = 1.0f;
	float bgmusicVolume = 1.0f;
	float SFXVolume = 1.0f;
	//float UISFXVolume = 1.0f;
	//float dialogueVolume = 1.0f;

	bool isMute
	{
		get
		{
			return masterVolume == 0.0f;
		}
	}

	public AudioMenu()
	{
		SFXVolume = 1.0f; // PlayerPrefs
		bgmusicVolume = 1.0f; // PlayerPrefs
		masterVolume = AudioListener.volume;
	}

	public override void ShowMe()
	{
		GUILayout.BeginArea(Utility.adjRect(box));

		// Master Volume
		GUILayout.Label("Master Volume");
		masterVolume = GUILayout.HorizontalSlider(masterVolume, 0.0f, 1.0f);

		if(GUILayout.Toggle(isMute, "Mute"))
		{
			masterVolume = 0.0f;
		}

		// SFX Volume
		GUILayout.Label("SFX Volume");
		SFXVolume = GUILayout.HorizontalSlider(SFXVolume, 0.0f, 1.0f);

		// Background Music Volume
		GUILayout.Label("Music Volume");
		bgmusicVolume = GUILayout.HorizontalSlider(bgmusicVolume, 0.0f, 1.0f);

		// Buttons
		if(GUILayout.Button("Apply"))
		{
			AudioListener.volume = masterVolume;
			// PlayerPrefs SFX & BgMusic
			// apply to all audio in scene
			OnChanged(EventArgs.Empty, 1);
		}

		if(GUILayout.Button("Back"))
		{
			OnChanged(EventArgs.Empty, 1);
		}

		GUILayout.EndArea();
	}
}
