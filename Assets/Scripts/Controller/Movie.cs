using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// Requires Unity Pro
public class Movie : MonoBehaviour
{
	public MovieTexture[] movie; // array of movie textures to play
	public int index = 0;
	public string nextLevel;
	// map of (cutscene to play/level to load) so everything's in one scene
	public Dictionary<int, string> map = new Dictionary<int, string>();
	//0 = happiness intro
	//1 = happiness outro
	//2 = sadness intro
	//3 = sadness outro
	//4 = fear intro
	//5 = fear outro
	//6 = anger intro
	//7 = anger outro

	// Use this for initialization
	void Start ()
	{
		//index = PlayerPrefs.GetInt("movieToPlay");
		if(movie != null)
		{
			// NOTE could use 3D plane (rotation.x = -90.0f) or draw directly on 2D GUI
			//this.renderer.material.mainTexture = movie;
			movie[index].Play(); // play at start
			// NOTE possibly have somewhere the ability to replay cutscenes after unlocked the first time
		}
		else
		{
			Debug.Log ("Missing Movie Texture, unable to play clip");
		}
	}
	
	// Update is called once per frame
	void Update ()
	{
		if(movie != null)
		{
			if(Time.time > movie[index].duration && Application.CanStreamedLevelBeLoaded(nextLevel))
			{
				// NOTE could give player ability to pause cutscene, but assuming how short they'll be, I don't think we'll need to
				Application.LoadLevel(nextLevel);
			}
		}

		// skip button
		if(Input.GetButtonDown("Jump") && Application.CanStreamedLevelBeLoaded(nextLevel))
		{
			Application.LoadLevel(nextLevel);
		}
	}

	void OnGUI()
	{
		GUILayout.Label("Cutscene: Press Spacebar to Skip.");

		if(movie != null)
		{
			// NOTE talk to Jyordana whether or not we're using a border
			GUI.DrawTexture(new Rect(0,0,Screen.width,Screen.height), movie[index], ScaleMode.StretchToFill);
		}
	}
}
